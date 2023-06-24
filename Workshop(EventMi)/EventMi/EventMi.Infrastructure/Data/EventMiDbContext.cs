using EventMi.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace EventMi.Infrastructure.Data;

public class EventMiDbContext : DbContext
{
    public EventMiDbContext()
    {
        
    }

    public EventMiDbContext(DbContextOptions<EventMiDbContext> options)
    :base(options)
    {
        
    }

    public DbSet<Event> Events { get; set; } = null!;
}