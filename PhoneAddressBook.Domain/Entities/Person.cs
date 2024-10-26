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
            Addresses = [];
        }
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<Address> Addresses { get; set; }
    }
}
