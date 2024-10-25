using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Domain.Entities
{
    public class Address
    {
        public Address()
        {
            PhoneNumbers = new List<PhoneNumber>();
        }
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public int Type { get; set; } 
        public string AddressDetail { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public Person Person { get; set; }
    }
}
