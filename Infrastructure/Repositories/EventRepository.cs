namespace Infrastructure.Repositories;

using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;

public class EventRepository(EventDBContext _eventDBContext) : IEventRepository
{
    private readonly DbSet<Event> _events = _eventDBContext.Set<Event>();

    public async Task CreateAsync(Event @event, CancellationToken cancelToken = default)
    {
        await _events.AddAsync(@event, cancelToken);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
    public async Task DeleteAsync(int id, CancellationToken cancelToken = default)
    {
        var @event = await _events.FirstOrDefaultAsync(e => e.Id == id, cancelToken);
        _events.Remove(@event);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }

    public async Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancelToken = default)
    {
        return await _events.ToListAsync(cancelToken);
    }

    public async Task<Event> GetByIdAsync(int id, CancellationToken cancelToken = default)
    {
        return await _events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancelToken);
    }

    public async Task<Event> GetByNameAsync(string name, CancellationToken cancelToken = default)
    {
        return await _events.AsNoTracking().FirstOrDefaultAsync(e => e.Name == name, cancelToken);
    }

    public async Task<IEnumerable<Event>> GetFilteredAsync(int pageNumber, int pageSize, CancellationToken cancelToken = default, DateTime? date = null, string location = null, string category = null)
    {
        IQueryable<Event> query = _events.AsQueryable();
        if (date.HasValue)
            query = query.Where(e => e.DateTimeEvent == date);
        if (!string.IsNullOrEmpty(location))
            query = query.Where(e => e.Location == location);
        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);
        query = query.Skip((pageNumber-1)*pageSize).Take(pageSize);

        return await query.ToListAsync(cancelToken);
    }

    public async Task UpdateAsync(Event entity, CancellationToken cancelToken = default)
    {
        _eventDBContext.Entry(entity).State = EntityState.Modified;
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
}
