using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class CreatePhoneNumberDto
{
    [Required]
    [Phone]
    public string Number { get; set; } = string.Empty;
}