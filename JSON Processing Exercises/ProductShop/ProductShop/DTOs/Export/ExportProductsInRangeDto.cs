using System.Text.Json.Serialization;

namespace ProductShop.DTOs.Export;

public class ExportProductsInRangeDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("seller")]
    public string SellerFullName { get; set; }
}