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
        Task<(IEnumerable<Person> People, int TotalCount)> GetPeopleAsync(int pageNumber, int pageSize, string filter);
        Task<Person> GetPersonByIdAsync(int id);
        Task CreatePersonAsync(Person person);
        Task UpdatePersonAsync(Person person);
        Task DeletePersonAsync(int id);
    }
}
