using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportProductsInRangeDto
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("seller")]
    public string SellerFullName { get; set; }
}