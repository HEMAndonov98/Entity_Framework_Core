using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Bet
{
    [Key]
    public int BetId { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.MoneyAmountSize})")]
    public decimal Amount { get; set; }

    public int Prediction { get; set; }

    public DateTime DateTime { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; }

    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }

    public Game Game { get; set; }
}