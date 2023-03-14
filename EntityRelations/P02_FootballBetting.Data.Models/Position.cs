using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models;

public class Position
{
    [Key]
    public int PositionId { get; set; }

    [Column(TypeName = $"NVARCHAR({ModelConstraints.PositionNameLength})")]
    public string Name { get; set; }

    [InverseProperty(nameof(Position))]
    public ICollection<Player> Players { get; set; }
    
}