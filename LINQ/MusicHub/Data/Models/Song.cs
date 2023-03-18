using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Common;
using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models;

public class Song
{
    public Song()
    {
        this.SongPerformers = new HashSet<SongPerformer>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ModelValidations.SongNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required] public TimeSpan Duration { get; init; }

    [Required] public DateTime CreatedOn { get; init; }

    [Required]
    public Genre Genre { get; set; }

    [ForeignKey(nameof(Album))]
    public int? AlbumId { get; set; }

    public Album Album { get; set; }
    
    [ForeignKey(nameof(Writer))]
    public int WriterId { get; set; }
    public Writer Writer { get; set; }

    public decimal Price { get; set; }
    
    [InverseProperty(nameof(Song))] 
    public ICollection<SongPerformer> SongPerformers { get; set; } = null!;
}