using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class CreatePhoneNumberDto
    {
        [Required]
        [Phone]
        public string Number { get; set; } = string.Empty;
    }
}
