using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportUsersDto
{
    public ExportUsersDto()
    {
        this.Products = new List<ExportProductDto>();
    }
    

    [JsonProperty("firstName")]
    public string? FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("age")]
    public int Age { get; set; }

    [JsonProperty("soldProducts")] 
    public int SoldProducts => this.Products.Count;

    [JsonProperty("products")]
    public ICollection<ExportProductDto> Products { get; set; }
}