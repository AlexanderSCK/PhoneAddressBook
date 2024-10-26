using PhoneAddressBook.Domain.Enums;
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
        public int Id { get; set; } 
        public int PersonId { get; set; } 
        public AddressType Type { get; set; }
        public string AddressDetail { get; set; } = string.Empty;
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
    }
}
