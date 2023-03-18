using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Writer
{
    public Writer()
    {
        this.Songs = new HashSet<Song>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ModelValidations.WriterNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ModelValidations.PsudonymMaxLength)]
    public string? Pseudonym { get; set; }

    [InverseProperty(nameof(Writer))]
    public ICollection<Song> Songs { get; set; }
}