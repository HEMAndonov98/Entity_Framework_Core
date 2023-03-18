using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Performer
{
    public Performer()
    {
        this.PerformerSongs = new HashSet<SongPerformer>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ModelValidations.PerformerNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(ModelValidations.PerformerNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public int Age { get; set; }

    [Required]
    public decimal NetWorth { get; set; }
    
    [InverseProperty(nameof(Performer))]
    public ICollection<SongPerformer> PerformerSongs { get; set; } = null!;
}