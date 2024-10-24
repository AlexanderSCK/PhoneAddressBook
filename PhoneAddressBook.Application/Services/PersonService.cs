using PhoneAddressBook.Application.Interfaces;
using PhoneAddressBook.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task CreatePersonAsync(Person person)
        {
            // Add any business logic here
            await _personRepository.AddAsync(person);
        }

        public async Task DeletePersonAsync(int id)
        {
            await _personRepository.DeleteAsync(id);
        }

        public async Task<Person> GetPersonByIdAsync(int id)
        {
            return await _personRepository.GetByIdAsync(id);
        }

        public async Task<(IEnumerable<Person> People, int TotalCount)> GetPeopleAsync(int pageNumber, int pageSize, string filter)
        {
            var people = await _personRepository.GetAllAsync(pageNumber, pageSize, filter);
            var totalCount = await _personRepository.GetTotalCountAsync(filter);
            return (people, totalCount);
        }

        public async Task UpdatePersonAsync(Person person)
        {
            await _personRepository.UpdateAsync(person);
        }
    }
}
