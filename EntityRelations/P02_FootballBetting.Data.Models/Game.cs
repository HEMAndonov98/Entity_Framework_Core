using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Game
{
    [Key]
    public int GameId { get; set; }

    public int HomeTeamGoals { get; set; }

    public int AwayTeamGoals { get; set; }

    public DateTime DateTime { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.BetRateSize})")]
    public decimal HomeTeamBetRate { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.BetRateSize})")]
    public decimal AwayTeamBetRate { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.BetRateSize})")]
    public decimal DrawBetRate { get; set; }

    public int Result { get; set; }

    [ForeignKey(nameof(HomeTeam))]
    public int HomeTeamId { get; set; }

    public Team HomeTeam { get; set; }

    [ForeignKey(nameof(AwayTeam))]
    public int AwayTeamId { get; set; }

    public Team AwayTeam { get; set; }

    [InverseProperty(nameof(Game))]
    public ICollection<PlayerStatistic> PlayersStatistics { get; set; }

    [InverseProperty(nameof(Game))]
    public ICollection<Bet> Bets { get; set; }
}