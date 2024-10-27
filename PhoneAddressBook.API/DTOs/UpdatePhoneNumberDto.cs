namespace PhoneAddressBook.API.DTOs;

public class UpdatePhoneNumberDto
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
}