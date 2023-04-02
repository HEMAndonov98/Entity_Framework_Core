using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto;

[XmlType("Creator")]
public class ImportCreatorDto
{
    [XmlElement("FirstName")]
    [Required]
    [MaxLength(7)]
    [MinLength(2)]
    public string FirstName { get; set; } = null!;

    [XmlElement("LastName")]
    [Required]
    [MaxLength(7)]
    [MinLength(2)]
    public string LastName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public virtual ImportBoardgameDto[] Boardgames { get; set; }
}