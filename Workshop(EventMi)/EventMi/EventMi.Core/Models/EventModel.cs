using System.ComponentModel.DataAnnotations;
using EventMi.Common.EntityConstraints;
using EventMi.Common.ErrorMessages;

namespace EventMi.Core.Models;

public class EventModel
{
    /// <summary>
    /// Event identifier
    /// </summary>

    public int Id { get; set; }

    /// <summary>
    /// The event name
    /// </summary>
    [Display(Name = "The event name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = EventModelErrors.RequiredNameField)]
    [StringLength(EventConstraints.EventNameMaxLength, MinimumLength = EventConstraints.EventNameMinLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Start date and time of event
    /// </summary>
    [Required]
    public DateTime Start { get; set; }

    /// <summary>
    /// End date and time of event
    /// </summary>
    [Required]
    public DateTime End { get; set; }

    /// <summary>
    /// Place where the event is being held
    /// </summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(EventConstraints.EventPlaceMaxLength, MinimumLength = EventConstraints.EventPlaceMinLength)]
    public string Place { get; set; } = null!;
}