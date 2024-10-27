using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Domain.Entities;

namespace PhoneAddressBook.Application.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task<(IEnumerable<Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter)
    {
        var result = await _personRepository.GetAllAsync(pageNumber, pageSize, filter);
        return (result.Persons, result.TotalCount);
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        var domainPerson = await _personRepository.GetByIdAsync(id);
        return domainPerson;
    }

    public async Task<Person> AddAsync(Person person)
    {
        await _personRepository.AddAsync(person);
        return person;
    }

    public async Task UpdateAddressesAsync(int personId, ICollection<UpdateAddressDto> addresses)
    {
        await _personRepository.UpdateAddressesAsync(personId, addresses);
    }

    public async Task DeleteAsync(int id)
    {
        await _personRepository.DeleteAsync(id);
    }
}