using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class User
{
    public int UserId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.UsernameLength})")]
    public string Username { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.PasswordLength})")]
    public string Password { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.EmailLength})")]
    public string Email { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.NameLength})")]
    public string? Name { get; set; }

    [Column(TypeName = $"DECIMAL({ModelConstraints.BalanceAmount})")]
    public decimal Balance { get; set; }

    [InverseProperty(nameof(User))]
    public ICollection<Bet> Bets { get; set; }
}