using System.Xml.Serialization;

namespace CarDealer.DTOs.Import;

[XmlType("Car")]
public class ImportCarDto
{
    [XmlElement("make")]
    public string Make { get; set; } = null!;

    [XmlElement("model")]
    public string Model { get; set; } = null!;

    [XmlElement("traveledDistance")]
    public long TraveledDistance { get; set; }
    
    [XmlArray("parts")]
    [XmlArrayItem("partId", typeof(ImportPartIdAttribute))]
    public ImportPartIdAttribute[]? Parts { get; set; }
}