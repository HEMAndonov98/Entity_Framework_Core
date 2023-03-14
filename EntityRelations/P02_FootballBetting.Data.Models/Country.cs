using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Country
{
    [Key]
    public int CountryId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.CountryNameLength})")]
    public string Name { get; set; }

    [InverseProperty(nameof(Country))]
    public ICollection<Town> Towns { get; set; }
}