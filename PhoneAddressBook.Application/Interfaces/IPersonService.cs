using PhoneAddressBook.API.DTOs;
using PhoneAddressBook.Domain.Entities;

namespace PhoneAddressBook.Application.Interfaces;

public interface IPersonService
{
    Task<(IEnumerable<Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter);
    Task<Person> GetByIdAsync(int id);
    Task<Person> AddAsync(Person person);
    Task UpdateAddressesAsync(int personId, ICollection<UpdateAddressDto> addresses);
    Task DeleteAsync(int id);
}