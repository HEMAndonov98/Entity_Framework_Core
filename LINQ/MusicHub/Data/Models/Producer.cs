using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Producer
{
    public Producer()
    {
        this.Albums = new HashSet<Album>();
    }
    
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(ModelValidations.ProducerNameMaxLength)]
    public string Name { get; set; } = null!;

    [MaxLength(ModelValidations.PsudonymMaxLength)]
    public string? Pseudonym { get; set; }

    [MaxLength(ModelValidations.PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }
    
    [InverseProperty(nameof(Producer))]
    public ICollection<Album> Albums { get; set; }
}