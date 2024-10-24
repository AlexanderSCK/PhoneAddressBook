using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int Type { get; set; } 
        public string AddressDetail { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public Person Person { get; set; }
    }
}
