using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportUsersSoldProductsDto
{
    public ExportUsersSoldProductsDto()
    {
        this.ProductsSold = new List<ExportSoldProductDto>();
    }

    [JsonProperty("firstName")]
    public string FirstName { get; set; }

    [JsonProperty("lastName")]
    public string LastName { get; set; }

    [JsonProperty("soldProducts")]
    public ICollection<ExportSoldProductDto> ProductsSold { get; set; }
}