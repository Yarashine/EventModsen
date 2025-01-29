namespace EventModsen.Domain.Interfaces;
using EventModsen.Infrastructure.DB.Models;

public interface IEventRepository
{
    public Task<bool> CreateAsync(Event @event);
    public Task<IEnumerable<Event>?> GetAllAsync();
    public Task<Event?> GetByIdAsync(int id);
    public Task<Event?> GetByNameAsync(string name);
    public Task<bool> DeleteAsync(int id);
    public Task<bool> UpdateAsync(Event entity);
    //public Task<bool> SaveAll();
    
}
