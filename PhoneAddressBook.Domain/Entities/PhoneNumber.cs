using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Domain.Entities
{
    public class PhoneNumber
    {
        public Guid Id { get; set; }
        public Guid AddressId { get; set; }
        public required string Number { get; set; }
        public  Address Address { get; set; }
    }
}
