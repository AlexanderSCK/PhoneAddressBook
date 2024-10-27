using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class UpdatePersonDto
{
    [MinLength(1, ErrorMessage = "At least one address is required.")]
    public ICollection<UpdateAddressDto> Addresses { get; set; } = new List<UpdateAddressDto>();
}