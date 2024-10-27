using System.Text.Json.Serialization;

namespace PhoneAddressBook.API.DTOs;

public class PersonDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; } 

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("addresses")]
    public List<AddressDto> Addresses { get; set; } = [];
}