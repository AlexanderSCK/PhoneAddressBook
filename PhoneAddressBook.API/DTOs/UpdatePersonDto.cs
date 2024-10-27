using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class UpdatePersonDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string FullName { get; set; } = string.Empty;

    [MinLength(1, ErrorMessage = "At least one address is required.")]
    public ICollection<UpdateAddressDto> Addresses { get; set; } = new List<UpdateAddressDto>();
}