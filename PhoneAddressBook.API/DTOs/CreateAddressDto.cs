using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class CreateAddressDto
    {
        [Required]
        [Range(1, 2, ErrorMessage = "Type must be either 1 (Home) or 2 (Business).")]
        public int Type { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string AddressDetail { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one phone number is required.")]
        public ICollection<CreatePhoneNumberDto> PhoneNumbers { get; set; } = new List<CreatePhoneNumberDto>();
    }
}
