namespace EventModsen.Infrastructure.DB.Repositories;

using EventModsen.Infrastructure.DB.Models;
using EventModsen.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;

public class EventRepository(EventDBContext _eventDBContext) : IEventRepository
{
    private readonly DbSet<Event> events = _eventDBContext.Set<Event>();

    public async Task<bool> CreateAsync(Event @event)
    {
        events.Add(@event);
        return await _eventDBContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var @event = await events.FirstOrDefaultAsync(e => e.Id == id);
        if (@event != null)
        {
            events.Remove(@event);
            return await _eventDBContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<IEnumerable<Event>?> GetAllAsync()
    {
        return await events.ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await events.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetByNameAsync(string name)
    {
        return await events.FirstOrDefaultAsync(e => e.Name == name);
    }


    public async Task<bool> UpdateAsync(Event entity)
    {
        _eventDBContext.Entry(entity).State = EntityState.Modified;
        return await _eventDBContext.SaveChangesAsync() > 0;
    }
}
