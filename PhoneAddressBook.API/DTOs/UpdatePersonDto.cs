using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class UpdatePersonDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public ICollection<UpdateAddressDto> Addresses { get; set; } = new List<UpdateAddressDto>();
    }
}
