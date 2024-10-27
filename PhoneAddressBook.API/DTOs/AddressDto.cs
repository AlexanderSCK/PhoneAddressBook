using System.Text.Json.Serialization;

namespace PhoneAddressBook.API.DTOs;

public class AddressDto
{
    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("phone_numbers")] 
    public List<string> PhoneNumbers { get; set; } = [];
}