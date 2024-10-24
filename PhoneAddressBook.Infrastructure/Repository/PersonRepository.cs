using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Infrastructure.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PhoneAddressBookDbContext _context;
        private readonly IMapper _mapper;

        public PersonRepository(PhoneAddressBookDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Domain.Entities.Person>> GetAllAsync(int pageNumber, int pageSize, string filter)
        {
            var sql = @"
                SELECT p.*, a.*, pn.*
                FROM Persons p
                LEFT JOIN Addresses a ON p.Id = a.Personid
                LEFT JOIN PhoneNumbers pn ON a.Id = pn.Addressid
                WHERE (@Filter IS NULL OR p.FullName ILIKE CONCAT('%', @Filter, '%'))
                ORDER BY p.Id
                LIMIT @PageSize OFFSET @Offset;
            ";

            var offset = (pageNumber - 1) * pageSize;
            var filterParam = new NpgsqlParameter("@Filter", string.IsNullOrEmpty(filter) ? (object)DBNull.Value : filter);
            var pageSizeParam = new NpgsqlParameter("@PageSize", pageSize);
            var offsetParam = new NpgsqlParameter("@Offset", offset);

            var infrastructurePersons = await _context.Persons
                .FromSqlRaw(sql, filterParam, pageSizeParam, offsetParam)
                .Include(p => p.Addresses)
                    .ThenInclude(a => a.Phonenumbers)
                .ToListAsync();

            var domainPersons = _mapper.Map<IEnumerable<Domain.Entities.Person>>(infrastructurePersons);
            return domainPersons;
        }

        public async Task<Domain.Entities.Person> GetByIdAsync(int id)
        {
            var sql = @"
                SELECT p.*, a.*, pn.*
                FROM Persons p
                LEFT JOIN Addresses a ON p.Id = a.Personid
                LEFT JOIN PhoneNumbers pn ON a.Id = pn.Addressid
                WHERE p.Id = @Id;
            ";

            var idParam = new NpgsqlParameter("@Id", id);

            var infrastructurePerson = await _context.Persons
                .FromSqlRaw(sql, idParam)
                .Include(p => p.Addresses)
                    .ThenInclude(a => a.Phonenumbers)
                .FirstOrDefaultAsync();

            if (infrastructurePerson == null)
                return null;

            // Map to Domain entity
            var domainPerson = _mapper.Map<Domain.Entities.Person>(infrastructurePerson);
            return domainPerson;
        }

        public async Task<int> GetTotalCountAsync(string filter)
        {
            var sql = @"
                SELECT COUNT(*)
                FROM Persons
                WHERE (@Filter IS NULL OR FullName ILIKE CONCAT('%', @Filter, '%'));
            ";

            var filterParam = new NpgsqlParameter("@Filter", string.IsNullOrEmpty(filter) ? (object)DBNull.Value : filter);

            var count = await _context.Persons
                .FromSqlRaw(sql, filterParam)
                .CountAsync();

            return count;
        }

        public async Task AddAsync(Domain.Entities.Person person)
        {
            var personEntity = _mapper.Map<Models.Person>(person);

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

            if (existingPerson == null)
                throw new KeyNotFoundException("Person not found.");

            _mapper.Map(person, existingPerson);

            foreach (var address in person.Addresses)
            {
                if (address.Id == 0)
                {
                    var newAddress = _mapper.Map<Models.Address>(address);
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
                                var newPhone = _mapper.Map<Models.Phonenumber>(phone);
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
            if (person == null)
                throw new KeyNotFoundException("Person not found.");

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
}
