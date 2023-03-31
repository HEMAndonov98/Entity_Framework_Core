using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportCategoryDto
{
    [JsonProperty("category")]
    public string CategoryName { get; set; }

    [JsonProperty("productsCount")]
    public int ProductsCount { get; set; }

    [JsonProperty("averagePrice")]
    public string AveragePrice { get; set; }

    [JsonProperty("totalRevenue")]
    public string TotalRevenue { get; set; }
}