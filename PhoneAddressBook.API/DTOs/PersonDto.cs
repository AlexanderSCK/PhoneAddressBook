using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PhoneAddressBook.API.DTOs
{
    public class PersonDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; } 

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("addresses")]
        public List<AddressDto> Addresses { get; set; }
    }
}
