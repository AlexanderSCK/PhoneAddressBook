using System.ComponentModel.DataAnnotations;

namespace PhoneAddressBook.API.DTOs;

public class UpdatePhoneNumberDto
{
    public int Id { get; set; }

    [Required] 
    [Phone] 
    public string Number { get; set; } = string.Empty;
}