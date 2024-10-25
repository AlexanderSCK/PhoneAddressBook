using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class UpdateAddressDto
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string AddressDetail { get; set; } = string.Empty;   
        public ICollection<UpdatePhoneNumberDto> PhoneNumbers { get; set; } = new List<UpdatePhoneNumberDto>();
    }
}
