using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.Domain.Entities
{
    public class PhoneNumber
    {
        public int Id { get; set; } 
        public int AddressId { get; set; } 
        public string Number { get; set; } = string.Empty;
    }
}
