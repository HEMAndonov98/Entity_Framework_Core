using System.ComponentModel.DataAnnotations;
using EventMi.Common.EntityConstraints;
using Microsoft.EntityFrameworkCore;

namespace EventMi.Infrastructure.Data.Models;
/// <summary>
/// Events
/// </summary>
[Comment("Events")]
public class Event
{
    /// <summary>
    /// Event identifier
    /// </summary>
    [Key]
    [Comment("Event identifier")]
    public int Id { get; set; }

    /// <summary>
    /// The event name
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(EventConstraints.EventNameMaxLength)]
    [Comment("The event name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Start date and time of event
    /// </summary>
    [Required]
    [Comment("Start date and time of event")]
    public DateTime Start { get; set; }

    /// <summary>
    /// End date and time of event
    /// </summary>
    [Required]
    [Comment("End date and time of event")]
    public DateTime End { get; set; }

    /// <summary>
    /// Place where the event is being held
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(EventConstraints.EventPlaceMaxLength)]
    [Comment("Place where the event is being held")]
    public string Place { get; set; } = null!;
}