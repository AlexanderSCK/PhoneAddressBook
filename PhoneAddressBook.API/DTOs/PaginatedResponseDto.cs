using System.Text.Json.Serialization;

namespace PhoneAddressBook.API.DTOs;

public class PaginatedResponseDto<T>
{
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }

    [JsonPropertyName("page_number")]
    public int PageNumber { get; set; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("people")] public List<PersonDto> People { get; set; } = [];
}