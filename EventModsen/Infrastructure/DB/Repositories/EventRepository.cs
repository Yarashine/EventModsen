namespace EventModsen.Infrastructure.DB.Repositories;

using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using System;

public class EventRepository(EventDBContext _eventDBContext) : IEventRepository
{
    private readonly DbSet<Event> _events = _eventDBContext.Set<Event>();

    public async Task<bool> CreateAsync(Event @event)
    {
        _events.Add(@event);
        return await _eventDBContext.SaveChangesAsync() > 0;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var @event = await _events.FirstOrDefaultAsync(e => e.Id == id);
        if (@event != null)
        {
            _events.Remove(@event);
            return await _eventDBContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<IEnumerable<Event>?> GetAllAsync()
    {
        return await _events.ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(int id)
    {
        return await _events.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event?> GetByNameAsync(string name)
    {
        return await _events.FirstOrDefaultAsync(e => e.Name == name);
    }

    public async Task<IEnumerable<Event>> GetFilteredAsync(int pageNumber, int pageSize, DateTime? date = null, string location = null, string category = null)
    {
        IQueryable<Event> query = _events.AsQueryable();
        if (date.HasValue)
            query = query.Where(e => e.DateTimeEvent == date);
        if (!string.IsNullOrEmpty(location))
            query = query.Where(e => e.Location == location);
        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);
        query = query.Skip((pageNumber-1)*pageSize).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<bool> UpdateAsync(Event entity)
    {
        _eventDBContext.Entry(entity).State = EntityState.Modified;
        return await _eventDBContext.SaveChangesAsync() > 0;
    }
}
