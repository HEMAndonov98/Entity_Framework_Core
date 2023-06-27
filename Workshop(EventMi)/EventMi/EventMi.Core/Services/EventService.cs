using EventMi.Core.Contracts;
using EventMi.Core.Models;

namespace EventMi.Core.Services;
/// <summary>
/// Implementation of Services
/// </summary>
public class EventService : IEventService
{
    /// <summary>
    /// Implementation for adding service
    /// </summary>
    /// <param name="model">Event data</param>
    public async Task AddEventAsync(EventModel model)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implementation for updating service
    /// </summary>
    /// <param name="model">Event data</param>
    public async Task UpdateEventAsync(EventModel model)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implementation for deletion service
    /// </summary>
    /// <param name="id">Event identifier</param>
    public async Task DeleteEventAsync(int id)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implementation for retrieving all events service
    /// </summary>
    /// <returns>Collection of events</returns>
    public async Task<IEnumerable<EventModel>> GetAllEventsAsync()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Implementation for retrieving a single event service 
    /// </summary>
    /// <param name="id">Event identifier</param>
    /// <returns>Single instance of an event</returns>
    public async Task<EventModel> GetEvent(int id)
    {
        throw new NotImplementedException();
    }
}