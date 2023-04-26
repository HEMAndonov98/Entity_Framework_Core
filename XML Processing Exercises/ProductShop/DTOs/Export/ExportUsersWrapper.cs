using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlRoot("Users")]
public class ExportUsersWrapper
{
    [XmlElement("count")]
    public int Count { get; set; }
    
    [XmlArray("users")]
    [XmlArrayItem("User", typeof(ExportUsersWithProductsDto))]
    public ExportUsersWithProductsDto[] Users { get; set; } = null!;
}