using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class UpdateAddressDto
{
    [Required]
    [Range(1, 2, ErrorMessage = "Type must be either 1 (Home) or 2 (Business).")]
    public int Type { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 5)]
    public string AddressDetail { get; set; } = string.Empty;   
    public ICollection<UpdatePhoneNumberDto> PhoneNumbers { get; set; } = new List<UpdatePhoneNumberDto>();
}