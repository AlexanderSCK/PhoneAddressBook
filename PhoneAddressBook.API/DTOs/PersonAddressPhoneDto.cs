using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class PersonAddressPhoneDto
    {
        // Person Fields
        public Guid PersonId { get; set; }
        public string FullName { get; set; } = string.Empty;

        // Address Fields
        public Guid? AddressId { get; set; }
        public Guid? AddressPersonId { get; set; }
        public int? AddressType { get; set; }
        public string AddressDetail { get; set; } = string.Empty;

        // PhoneNumber Fields
        public Guid? PhoneNumberId { get; set; }
        public Guid? PhoneNumberAddressId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
