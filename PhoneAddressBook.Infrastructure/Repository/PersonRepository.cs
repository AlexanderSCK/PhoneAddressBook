using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.API.Exceptions;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Infrastructure.Models;

namespace PhoneAddressBook.Infrastructure.Repository;

public class PersonRepository : IPersonRepository
{
    private readonly PostgresContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PersonRepository> _logger;
    public PersonRepository(PostgresContext context, IMapper mapper, ILogger<PersonRepository> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<Domain.Entities.Person> Persons, int TotalCount)> GetAllAsync(int pageNumber,
        int pageSize, string filter)
    {
        _logger.LogInformation("GetAllAsync called with PageNumber: {PageNumber}, PageSize: {PageSize}, Filter: {Filter}", pageNumber, pageSize, filter);

        try
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

            _logger.LogDebug("Executing SQL: {Sql} with Parameters: {@Parameters}", sql, new { filterParam, pageSizeParam, offsetParam });

            var rawResults = await _context.PersonAddressPhoneDtos
                .FromSqlRaw(sql, filterParam, pageSizeParam, offsetParam)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogDebug("Raw results retrieved: {Count} records", rawResults.Count);

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

            _logger.LogInformation("Total domain persons mapped: {Count}", domainPersons.Count);

            var totalCount = await GetTotalCountAsync(filter);
            _logger.LogInformation("Total count retrieved: {TotalCount}", totalCount);

            return (domainPersons, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetAllAsync with PageNumber: {PageNumber}, PageSize: {PageSize}, Filter: {Filter}", pageNumber, pageSize, filter);
            throw; 
        }
    }

    public async Task<Domain.Entities.Person> GetByIdAsync(int id)
    {
        _logger.LogInformation("GetByIdAsync called with Person ID: {PersonId}", id);

        try
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

            _logger.LogDebug("Executing SQL: {Sql} with Parameter: {@IdParam}", sql, idParam);

            var rawResults = await _context.PersonAddressPhoneDtos
                .FromSqlRaw(sql, idParam)
                .AsNoTracking()
                .ToListAsync();

            _logger.LogDebug("Raw results retrieved: {Count} records for Person ID: {PersonId}", rawResults.Count, id);

            if (!rawResults.Any())
            {
                throw new NotFoundException($"Person with ID {id} not found.");
            }

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

            _logger.LogInformation("Person retrieved successfully with ID: {PersonId}", id);

            return domainPerson;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetByIdAsync with Person ID: {PersonId}", id);
            throw; 
        }
    }
    private async Task<int> GetTotalCountAsync(string filter)
    {
        _logger.LogInformation("GetTotalCountAsync called with Filter: {Filter}", filter);
        try
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                 var count = await _context.Persons.CountAsync();
                _logger.LogInformation("Total count without filter: {Count}", count);
                return count;
            }

            return await _context.Persons
                .Where(p => EF.Functions.ILike(p.Fullname, $"%{filter}%"))
                .CountAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred in GetTotalCountAsync with Filter: {Filter}", filter);
            throw;
        }
    }

    public async Task AddAsync(Domain.Entities.Person person)
    {
        _logger.LogInformation("AddAsync called to add a new Person: {@Person}", person);

        try
        {
            var personEntity = _mapper.Map<Person>(person);
            _context.Persons.Add(personEntity);

            await _context.SaveChangesAsync();
            _logger.LogInformation("Person added successfully with ID: {PersonId}", personEntity.Id);

            _mapper.Map(personEntity, person);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new Person: {@Person}", person);
            throw; 
        }
    }

    public async Task UpdateAddressesAsync(int personId, ICollection<UpdateAddressDto> addressesDto)
    {
        _logger.LogInformation("UpdateAddressesAsync called for Person ID: {PersonId} with {AddressCount} addresses.", personId, addressesDto.Count);

        try
        {
            var person = await _context.Persons
                .Include(p => p.Addresses)
                .ThenInclude(a => a.Phonenumbers)
                .FirstOrDefaultAsync(p => p.Id == personId);

            if (person == null)
            {
                throw new NotFoundException($"Person with ID {personId} not found.");
            }

            _context.Phonenumbers.RemoveRange(person.Addresses.SelectMany(a => a.Phonenumbers));

            _context.Addresses.RemoveRange(person.Addresses);

            foreach (var addressDto in addressesDto)
            {
                _logger.LogDebug("Adding new Address: {@AddressDto} for Person ID: {PersonId}", addressDto, personId);
                var newAddress = new Address
                {
                    Type = addressDto.Type,
                    Address1 = addressDto.AddressDetail,
                    Personid = personId
                };

                foreach (var phoneDto in addressDto.PhoneNumbers)
                {
                    _logger.LogDebug("Adding new Phone Number: {PhoneNumber} to Address: {@Address}", phoneDto.Number, newAddress);
                    var newPhoneNumber = new Phonenumber
                    {
                        Phonenumber1 = phoneDto.Number,
                        Address = newAddress
                    };
                    _context.Phonenumbers.Add(newPhoneNumber);
                }

                _context.Addresses.Add(newAddress);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Addresses and Phone Numbers updated successfully for Person ID: {PersonId}", personId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating Addresses for Person ID: {PersonId}", personId);
            throw; 
        }
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation("DeleteAsync called to delete Person with ID: {PersonId}", id);

        try
        {
            var person = await _context.Persons.FindAsync(id);
            if (person is null)
            {
                throw new NotFoundException($"Person with id: {id} not found.");
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Person with ID: {PersonId} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting Person with ID: {PersonId}", id);
            throw;
        }
    }
}