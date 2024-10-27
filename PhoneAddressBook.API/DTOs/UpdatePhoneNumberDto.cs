using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class UpdatePhoneNumberDto
{
    [Required] 
    [Phone] 
    public string Number { get; set; } = string.Empty;
}