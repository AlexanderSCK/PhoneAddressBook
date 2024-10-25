using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Domain.Entities
{
    public class Person
    {
        public Person()
        {
            Addresses = new List<Address>();
        }
        public Guid Id { get; set; }
        public required string FullName { get; set; }
        public ICollection<Address> Addresses { get; set; }
    }
}
