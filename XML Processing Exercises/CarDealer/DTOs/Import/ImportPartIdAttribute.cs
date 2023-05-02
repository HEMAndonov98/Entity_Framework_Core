using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;
[XmlType("partId")]
public class ImportPartIdAttribute
{
    [XmlAttribute("id")]
    public int Id { get; set; }
}