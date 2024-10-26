using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhoneAddressBook.API.Exceptions;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Infrastructure.Models;

namespace PhoneAddressBook.Infrastructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PostgresContext _context;
        private readonly IMapper _mapper;

        public PersonRepository(PostgresContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<Domain.Entities.Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter)
        {
            var sql = @"
                SELECT 
                    p.id AS PersonId,
                    p.fullname AS FullName,
                    a.id AS AddressId,
                    a.personid AS AddressPersonId,
                    a.type AS AddressType,
                    a.address AS AddressDetail,
                    pn.id AS PhoneNumberId,
                    pn.addressid AS PhoneNumberAddressId,
                    pn.phonenumber AS PhoneNumber
                FROM persons p
                LEFT JOIN addresses a ON p.id = a.personid
                LEFT JOIN phonenumbers pn ON a.id = pn.addressid
                WHERE (@Filter IS NULL OR p.fullname ILIKE '%' || @Filter || '%')
                ORDER BY p.id
                LIMIT @PageSize OFFSET @Offset
            ";
            var offset = (pageNumber - 1) * pageSize;
            var filterParam = new NpgsqlParameter("@Filter", NpgsqlTypes.NpgsqlDbType.Varchar)
            {
                Value = string.IsNullOrWhiteSpace(filter) ? DBNull.Value : filter
            };

            var pageSizeParam = new NpgsqlParameter("@PageSize", NpgsqlTypes.NpgsqlDbType.Integer)
            {
                Value = pageSize
            };

            var offsetParam = new NpgsqlParameter("@Offset", NpgsqlTypes.NpgsqlDbType.Integer)
            {
                Value = offset
            };

            var rawResults = await _context.PersonAddressPhoneDtos
                .FromSqlRaw(sql, filterParam, pageSizeParam, offsetParam)
                .AsNoTracking()
                .ToListAsync();

            var domainPersons = rawResults
                .GroupBy(r => new { r.PersonId, r.FullName })
                .Select(g => new Domain.Entities.Person
                {
                    Id = g.Key.PersonId,
                    FullName = g.Key.FullName,
                    Addresses = g
                        .Where(r => r.AddressId.HasValue)
                        .GroupBy(r => new { r.AddressId, r.AddressPersonId, r.AddressType, r.AddressDetail })
                        .Select(ag => new Domain.Entities.Address
                        {
                            Id = ag.Key.AddressId.Value,
                            PersonId = ag.Key.AddressPersonId.Value,
                            Type = (Domain.Enums.AddressType)(ag.Key.AddressType ?? 1),
                            AddressDetail = ag.Key.AddressDetail,
                            PhoneNumbers = ag
                                .Where(r => r.PhoneNumberId.HasValue)
                                .Select(rp => new Domain.Entities.PhoneNumber
                                {
                                    Id = rp.PhoneNumberId.Value,
                                    AddressId = rp.PhoneNumberAddressId.Value,
                                    Number = rp.PhoneNumber
                                }).ToList()
                        }).ToList()
                }).ToList();

            var totalCount = await GetTotalCountAsync(filter);
            return (domainPersons, totalCount);
        }

        public async Task<Domain.Entities.Person> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT 
                    p.id AS PersonId,
                    p.fullname AS FullName,
                    a.id AS AddressId,
                    a.personid AS AddressPersonId,
                    a.type AS AddressType,
                    a.address AS AddressDetail,
                    pn.id AS PhoneNumberId,
                    pn.addressid AS PhoneNumberAddressId,
                    pn.phonenumber AS PhoneNumber
                FROM persons p
                LEFT JOIN addresses a ON p.id = a.personid
                LEFT JOIN phonenumbers pn ON a.id = pn.addressid
                WHERE p.id = @Id
            ";

            var idParam = new NpgsqlParameter("@Id", id);

            var rawResults = await _context.PersonAddressPhoneDtos
                .FromSqlRaw(sql, idParam)
                .AsNoTracking()
                .ToListAsync();

            var domainPerson = new Domain.Entities.Person
            {
                Id = rawResults.First().PersonId,
                FullName = rawResults.First().FullName,
                Addresses = rawResults
                    .Where(r => r.AddressId.HasValue)
                    .GroupBy(r => new { r.AddressId, r.AddressPersonId, r.AddressType, r.AddressDetail })
                    .Select(g => new Domain.Entities.Address
                    {
                        Id = g.Key.AddressId.Value,
                        PersonId = g.Key.AddressPersonId.Value,
                        Type = (Domain.Enums.AddressType)(g.Key.AddressType ?? 0),
                        AddressDetail = g.Key.AddressDetail,
                        PhoneNumbers = g
                            .Where(r => r.PhoneNumberId.HasValue)
                            .Select(rp => new Domain.Entities.PhoneNumber
                            {
                                Id = rp.PhoneNumberId.Value,
                                AddressId = rp.PhoneNumberAddressId.Value,
                                Number = rp.PhoneNumber
                            }).ToList()
                    }).ToList()
            };

            return domainPerson;
        }

        public async Task<int> GetTotalCountAsync(string filter)
        {
            var sql = @"
               SELECT COUNT(*)
               FROM persons
               WHERE (@Filter IS NULL OR fullname ILIKE '%' || @Filter || '%')";

            var filterParam = new NpgsqlParameter("@Filter", NpgsqlTypes.NpgsqlDbType.Varchar)
            {
                Value = string.IsNullOrWhiteSpace(filter) ? DBNull.Value : filter
            };

            var count = await _context.Persons
                .FromSqlRaw(sql, filterParam)
                .CountAsync();

            return count;
        }

        public async Task AddAsync(Domain.Entities.Person person)
        {
            var personEntity = _mapper.Map<Person>(person);
            _context.Persons.Add(personEntity);
            await _context.SaveChangesAsync();

            _mapper.Map(personEntity, person);
        }

        public async Task UpdateAsync(Domain.Entities.Person person)
        {
            var existingPerson = await _context.Persons
                .Include(p => p.Addresses)
                    .ThenInclude(a => a.Phonenumbers)
                .FirstOrDefaultAsync(p => p.Id == person.Id);

            if (existingPerson is null)
            {
                throw new NotFoundException($"Person with id: {person.Id} not found.");
            }

            _mapper.Map(person, existingPerson);

            foreach (var address in person.Addresses)
            {
                if (address.Id == 0)
                {
                    var newAddress = _mapper.Map<Address>(address);
                    existingPerson.Addresses.Add(newAddress);
                }
                else
                {
                    var existingAddress = existingPerson.Addresses.FirstOrDefault(a => a.Id == address.Id);
                    if (existingAddress != null)
                    {
                        _mapper.Map(address, existingAddress);

                        foreach (var phone in address.PhoneNumbers)
                        {
                            if (phone.Id == 0)
                            {
                                var newPhone = _mapper.Map<Phonenumber>(phone);
                                existingAddress.Phonenumbers.Add(newPhone);
                            }
                            else
                            {
                                var existingPhone = existingAddress.Phonenumbers.FirstOrDefault(pn => pn.Id == phone.Id);
                                if (existingPhone != null)
                                {
                                    _mapper.Map(phone, existingPhone);
                                }
                            }
                        }

                        var updatedPhoneIds = address.PhoneNumbers.Where(p => p.Id != 0).Select(p => p.Id).ToList();
                        var phonesToRemove = existingAddress.Phonenumbers.Where(pn => !updatedPhoneIds.Contains(pn.Id)).ToList();
                        foreach (var phone in phonesToRemove)
                        {
                            existingAddress.Phonenumbers.Remove(phone);
                        }
                    }
                }
            }

            var updatedAddressIds = person.Addresses.Where(a => a.Id != 0).Select(a => a.Id).ToList();
            var addressesToRemove = existingPerson.Addresses.Where(a => !updatedAddressIds.Contains(a.Id)).ToList();
            foreach (var address in addressesToRemove)
            {
                existingPerson.Addresses.Remove(address);
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person is null)
            {
                throw new NotFoundException($"Person with id: {id} not found.");
            }
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
