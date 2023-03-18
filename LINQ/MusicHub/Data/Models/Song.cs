using System.ComponentModel.DataAnnotations;
using MusicHub.Common;
using MusicHub.Data.Models.Enums;

namespace MusicHub.Data.Models;

public class Song
{
    [Key]
    public int SongId { get; set; }

    [Required]
    [MaxLength(ModelValidations.SongNameMaxLength)]
    public string Name { get; set; }

    [Required]
    public TimeSpan Duration { get; set; }

    [Required]
    public DateTime CreatedOn { get; set; }

    [Required]
    public Genres Genre { get; set; }

    public int AlbumId { get; set; }
    
    //TODO Create Foreign key and navigation property to Album

    public int WriterId { get; set; }
    
    //TODO Create Foreign key and navigation property to Writer

    public decimal Price { get; set; }
}