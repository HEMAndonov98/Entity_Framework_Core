using Newtonsoft.Json;

namespace ProductShop.DTOs.Export;

public class ExportUsersWithProductsDto
{
    public ExportUsersWithProductsDto()
    {
        this.Users = new List<ExportUsersDto>();
    }

    [JsonProperty("usersCount")] 
    public int UsersCount => this.Users.Count;

    [JsonProperty("users")]
    public ICollection<ExportUsersDto> Users { get; set; }
}