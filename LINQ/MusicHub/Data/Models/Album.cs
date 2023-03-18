using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Album
{
    public Album()
    {
        this.Songs = new HashSet<Song>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ModelValidations.AlbumNameMaxLength)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    public decimal Price => this.Songs
        .Select(s => s.Price)
        .Sum();
    
    [ForeignKey(nameof(Producer))]
    public int? ProducerId { get; set; }
    public Producer Producer { get; set; } = null!;
    
    [InverseProperty(nameof(Album))]
    public ICollection<Song> Songs { get; set; }
}