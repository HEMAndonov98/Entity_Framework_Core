using System.ComponentModel.DataAnnotations;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Album
{
    [Key]
    public int AlbumId { get; set; }

    [Required]
    [MaxLength(ModelValidations.AlbumNameMaxLength)]
    public string Name { get; set; }

    [Required]
    public DateTime ReleaseDate { get; set; }

    //TODO public decimal Price = Songs.Price.Sum()

    public int ProducerId { get; set; }
    
    //TODO Foreign key for ProducerId and navigational property
    
    //TODO Create a collection navigational property of type song
}