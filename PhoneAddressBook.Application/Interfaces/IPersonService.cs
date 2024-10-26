using PhoneAddressBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Application.Interfaces
{
    public interface IPersonService
    {
        Task<(IEnumerable<Person> Persons, int TotalCount)> GetAllAsync(int pageNumber, int pageSize, string filter);
        Task<Person> GetByIdAsync(int id);
        Task<int> GetTotalCountAsync(string filter);
        Task<Person> AddAsync(Person person);
        Task<Person> UpdateAsync(Person person);
        Task DeleteAsync(int id);
    }
}
