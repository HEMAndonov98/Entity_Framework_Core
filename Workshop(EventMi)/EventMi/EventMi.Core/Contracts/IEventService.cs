using EventMi.Core.Models;

namespace EventMi.Core.Contracts;

public interface IEventService
{
    /// <summary>
    /// Service for adding an event
    /// </summary>
    /// <param name="model">Event data</param>
    /// <returns></returns>
    public Task AddEventAsync(EventModel model);

    /// <summary>
    /// Service for updating an event
    /// </summary>
    /// <param name="model">Event data</param>
    /// <returns></returns>
    public Task UpdateEventAsync(EventModel model);

    /// <summary>
    /// Service for deleting an event
    /// </summary>
    /// <param name="id">Event identifier</param>
    /// <returns></returns>
    public Task DeleteEventAsync(int id);

    /// <summary>
    /// Service for retrieving all events
    /// </summary>
    /// <returns>Collection of Events</returns>
    public Task<IEnumerable<EventModel>> GetAllEventsAsync();

    /// <summary>
    /// Service for retrieving an event 
    /// </summary>
    /// <param name="id">Event identifier</param>
    /// <returns></returns>
    public Task<EventModel> GetEvent(int id);
}