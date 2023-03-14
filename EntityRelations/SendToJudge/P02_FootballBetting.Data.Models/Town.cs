using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Town
{
    [Key]
    public int TownId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.TownNameLength})")]
    public string Name { get; set; }

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }

    public Country Country { get; set; }

    [InverseProperty(nameof(Town))]
    public ICollection<Team> Teams { get; set; }

    [InverseProperty(nameof(Town))]
    public ICollection<Player> Players { get; set; }
}