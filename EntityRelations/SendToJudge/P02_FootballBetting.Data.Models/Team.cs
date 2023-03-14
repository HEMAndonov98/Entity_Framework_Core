using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Team
{
    [Key]
    public int TeamId { get; set; }
    
    [Column(TypeName = $"NVARCHAR({ModelConstraints.TeamNameLength})")]
    public string Name { get; set; }

    [Column(TypeName = $"VARCHAR({ModelConstraints.MaxUrlLength})")]
    public string? LogoUrl { get; set; }

    [Column(TypeName = $"VARCHaR({ModelConstraints.MaimumNumberOfInitials})")]
    public string Initials { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.BudgetDecimalSize})")]
    public decimal Budget { get; set; }

    public int PrimaryKitColorId { get; set; }
    public Color PrimaryKitColor { get; set; }

    public int SecondaryKitColorId { get; set; }
    public Color SecondaryKitColor { get; set; }

    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }

    public Town Town { get; set; }

    [InverseProperty(nameof(Team))]
    public ICollection<Player> Players { get; set; }

    [InverseProperty("HomeTeam")]
    public ICollection<Game> HomeGames { get; set; }

    [InverseProperty("AwayTeam")]
    public ICollection<Game> AwayGames { get; set; }
}