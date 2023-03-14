using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Player
{
    [Key]
    public int PlayerId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.PlayerNameLength})")]
    public string Name { get; set; }

    public int SquadNumber { get; set; }

    public bool IsInjured { get; set; }

    [ForeignKey(nameof(Team))]
    public int TeamId { get; set; }

    public Team Team { get; set; }

    [ForeignKey(nameof(Position))]
    public int PositionId { get; set; }

    public Position Position { get; set; }

    [ForeignKey(nameof(Town))]
    public int TownId { get; set; }

    public Town Town { get; set; }

    [InverseProperty(nameof(Player))]
    public ICollection<PlayerStatistic> PlayersStatistics { get; set; }
}