using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Color
{
    [Key]
    public int ColorId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.ColorNameLength})")]
    public string Name { get; set; }

    [InverseProperty(nameof(Team.PrimaryKitColor))]
    public ICollection<Team> PrimaryKitTeams { get; set; }

    [InverseProperty(nameof(Team.SecondaryKitColor))]
    public ICollection<Team> SecondaryKitTeams { get; set; }
}