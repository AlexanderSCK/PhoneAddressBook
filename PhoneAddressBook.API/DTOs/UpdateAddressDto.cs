namespace PhoneAddressBook.API.DTOs;

public class UpdateAddressDto
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string AddressDetail { get; set; } = string.Empty;   
    public ICollection<UpdatePhoneNumberDto> PhoneNumbers { get; set; } = new List<UpdatePhoneNumberDto>();
}