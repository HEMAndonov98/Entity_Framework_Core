using EventMi.Core.Contracts;
using EventMi.Core.Models;
using EventMi.Infrastructure.Common.RepositoryContracts;
using EventMi.Infrastructure.Data;
using EventMi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EventMi.Core.Services;
/// <summary>
/// Implementation of Services
/// </summary>
public class EventService : IEventService
{
    private readonly IRepository<Event> _repository;
    private readonly EventMiDbContext _context;

    public EventService(IRepository<Event> repository, EventMiDbContext context)
    {
        this._repository = repository;
        this._context = context;
    }

    /// <summary>
    /// Implementation for adding service
    /// </summary>
    /// <param name="model">Event data</param>
    public async Task AddEventAsync(EventModel model)
    {
        Event newEvent = new Event()
        {
            Name = model.Name,
            Start = model.Start,
            End = model.End,
            Place = model.Place
        };

        await this._repository.AddAsync(newEvent);
        await this._repository.SaveChangesAsync();
    }

    /// <summary>
    /// Implementation for updating service
    /// </summary>
    /// <param name="model">Event data</param>
    public async Task UpdateEventAsync(EventModel model)
    {
        Event entity = new Event()
        {
            Id = model.Id,
            Name = model.Name,
            Start = model.Start,
            End = model.End,
            Place = model.Place
        };

        this._repository.Update(entity);
        await this._repository.SaveChangesAsync();
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
        var events = await this._repository.AllAsNoTracking()
            .Select(e => new EventModel()
            {
                Id = e.Id,
                Name = e.Name,
                Start = e.Start,
                End = e.End,
                Place = e.Place
            })
            .ToArrayAsync();
        return events;
    }

    /// <summary>
    /// Implementation for retrieving a single event service 
    /// </summary>
    /// <param name="id">Event identifier</param>
    /// <returns>Single instance of an event</returns>
    public async Task<EventModel> GetEvent(int id)
    {
        Event entity = await this._context.Events.FindAsync(id);
        EventModel eventModel = new EventModel()
        {
            Id = entity.Id,
            Name = entity.Name,
            Start = entity.Start,
            End = entity.End,
            Place = entity.Place
        };

        return eventModel;
    }
}