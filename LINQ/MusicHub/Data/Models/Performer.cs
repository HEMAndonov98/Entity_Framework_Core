using System.ComponentModel.DataAnnotations;
using MusicHub.Common;

namespace MusicHub.Data.Models;

public class Performer
{
    [Key]
    public int PerformerId { get; set; }

    [Required]
    [MaxLength(ModelValidations.PerformerNameMaxLength)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(ModelValidations.PerformerNameMaxLength)]
    public string LastName { get; set; }

    [Required]
    public int Age { get; set; }

    [Required]
    public decimal NetWorth { get; set; }
    
    //TODO Create ICollection of SongPerformers for many to many relation
}