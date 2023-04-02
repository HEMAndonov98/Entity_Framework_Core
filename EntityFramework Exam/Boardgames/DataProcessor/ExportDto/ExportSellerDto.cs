using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ExportDto;

public class ExportSellerDto
{
    public ExportSellerDto()
    {
        this.Boardgames = new List<ExportBoardgameDto>();
    }
    
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("website")]
    public string Website { get; set; } = null!;

    [JsonProperty("boardgames")]
    public ICollection<ExportBoardgameDto> Boardgames { get; set; }
}