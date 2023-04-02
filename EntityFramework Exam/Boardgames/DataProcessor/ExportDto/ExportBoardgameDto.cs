using Newtonsoft.Json;

namespace Boardgames.DataProcessor.ExportDto;

public class ExportBoardgameDto
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;
    [JsonProperty("rating")]
    public double Rating { get; set; }
    [JsonProperty("category")]
    public string CategoryType { get; set; }
    [JsonProperty("mechanics")]
    public string Mechanics { get; set; } = null!;
}