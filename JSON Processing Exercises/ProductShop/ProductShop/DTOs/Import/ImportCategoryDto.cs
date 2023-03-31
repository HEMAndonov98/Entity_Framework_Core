using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Import;

public class ImportCategoryDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}