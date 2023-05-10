using System.Xml.Serialization;
using CarDealer.Models;

namespace CarDealer.DTOs.Export;
[XmlType("customer")]
public class ExportCustomerDto
{
    [XmlAttribute("full-name")]
    public string Name { get; set; } = null!;
    [XmlAttribute("bought-cars")]
    public int BoughtCars { get; set; }

    [XmlAttribute("spent-money")] 
    public decimal SpentMoney { get; set; }

    
}