namespace EventModsen.Domain.Interfaces;
using EventModsen.Domain.Entities;

public interface IEventRepository
{
    public Task CreateAsync(Event @event);
    public Task<IEnumerable<Event>> GetAllAsync();
    public Task<IEnumerable<Event>> GetFilteredAsync(int pageNumber, int pageSize, DateTime? date = null, string? location = null, string? category = null);
    public Task<Event> GetByIdAsync(int id);
    public Task<Event> GetByNameAsync(string name);
    public Task DeleteAsync(int id);
    public Task UpdateAsync(Event entity);
    
}
