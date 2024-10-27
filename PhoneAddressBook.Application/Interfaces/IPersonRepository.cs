using PhoneAddressBook.Domain.Entities;

namespace PhoneAddressBook.Application.Interfaces;

public interface IPersonRepository
{
    Task<(IEnumerable<Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter);
    Task<Person> GetByIdAsync(int id);
    Task<int> GetTotalCountAsync(string filter);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}