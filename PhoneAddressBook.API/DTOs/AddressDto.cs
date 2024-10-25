using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public int Type { get; set; } 
        public string AddressDetail { get; set; } = string.Empty;
        public ICollection<PhoneNumberDto> PhoneNumbers { get; set; } = new List<PhoneNumberDto>();
    }
}
