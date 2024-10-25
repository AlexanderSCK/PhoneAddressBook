using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class PersonDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<AddressDto> Addresses { get; set; } = new List<AddressDto>();
    }
}
