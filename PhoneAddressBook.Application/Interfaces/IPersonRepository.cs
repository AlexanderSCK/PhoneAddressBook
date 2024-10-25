using PhoneAddressBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Application.Interfaces
{
    public interface IPersonRepository
    {
        Task<(IEnumerable<Domain.Entities.Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter);
        Task<Person> GetByIdAsync(Guid id);
        Task<int> GetTotalCountAsync(string filter);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(Guid id);
    }
}
