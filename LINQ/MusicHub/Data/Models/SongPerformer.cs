using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicHub.Data.Models;

public class SongPerformer
{
    [Key]
    [ForeignKey(nameof(Song))]
    public int SongId { get; set; }

    public Song Song { get; init; } = null!;

    [Key]
    [ForeignKey(nameof(Performer))]
    public int PerformerId { get; set; }

    public Performer Performer { get; init; } = null!;
}