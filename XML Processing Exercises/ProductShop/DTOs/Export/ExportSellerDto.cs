using System.Xml.Serialization;

namespace ProductShop.DTOs.Export;

[XmlType("User")]
public class ExportSellerDto
{
    [XmlElement("firstName")]
    public string? FirstName { get; set; }

    [XmlElement("lastName")]
    public string LastName { get; set; }

    [XmlArray("soldProducts")]
    [XmlArrayItem("Product", typeof(ExportSellerProductDto))]
    public ExportSellerProductDto[] SoldProducts { get; set; }

}