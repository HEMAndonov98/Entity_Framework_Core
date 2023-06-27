using System.ComponentModel.DataAnnotations;
using EventMi.Common.EntityConstraints;
using EventMi.Common.ErrorMessages;

namespace EventMi.Core.Models;

/// <summary>
/// Event
/// </summary>
public class EventModel : EventBaseModel
{
    /// <summary>
    /// The event name
    /// </summary>
    [Display(Name = "The event name")]
    [Required(AllowEmptyStrings = false, ErrorMessage = EventModelErrors.RequiredField)]
    [StringLength(EventConstraints.EventNameMaxLength, MinimumLength = EventConstraints.EventNameMinLength
    , ErrorMessage = EventModelErrors.InvalidFieldLength)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Start date and time of event
    /// </summary>
    [Display(Name = "Start date and time of event")]
    [Required(AllowEmptyStrings = false, ErrorMessage = EventModelErrors.RequiredField)]
    public DateTime Start { get; set; }

    /// <summary>
    /// End date and time of event
    /// </summary>
    [Display(Name = "End date and time of event")]
    [Required(AllowEmptyStrings = false, ErrorMessage = EventModelErrors.RequiredField)]
    public DateTime End { get; set; }

    /// <summary>
    /// Place where the event is being held
    /// </summary>
    [Display(Name = "Place where the event is being held")]
    [Required(AllowEmptyStrings = false, ErrorMessage = EventModelErrors.RequiredField)]
    [StringLength(EventConstraints.EventPlaceMaxLength, MinimumLength = EventConstraints.EventPlaceMinLength,
        ErrorMessage = EventModelErrors.InvalidFieldLength)]
    public string Place { get; set; } = null!;
}