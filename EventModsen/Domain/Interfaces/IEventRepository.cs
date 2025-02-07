namespace EventModsen.Domain.Interfaces;
using EventModsen.Domain.Entities;

public interface IEventRepository
{
    public Task CreateAsync(Event @event, CancellationToken cancelToken = default);
    public Task<IEnumerable<Event>> GetAllAsync(CancellationToken cancelToken = default);
    public Task<IEnumerable<Event>> GetFilteredAsync(int pageNumber, int pageSize, CancellationToken cancelToken = default, DateTime? date = null, string? location = null, string? category = null);
    public Task<Event> GetByIdAsync(int id, CancellationToken cancelToken = default);
    public Task<Event> GetByNameAsync(string name, CancellationToken cancelToken = default);
    public Task DeleteAsync(int id, CancellationToken cancelToken = default);
    public Task UpdateAsync(Event entity, CancellationToken cancelToken = default);
    
}
