namespace EventModsen.Domain.Interfaces;
using EventModsen.Domain.Entities;

public interface IMemberRepository
{
    public Task AddToEventAsync(int memberId, int eventId);
    public Task<IEnumerable<Member>> GetAllByEventIdAsync(int eventId);
    public Task<Member> GetByIdAsync(int id);
    public Task<Member> GetByEmailAsync(string email);
    public Task DeleteFromEventAsync(int memberId, int eventId);
    public Task AddAsync (Member member);
    public Task UpdateRefreshAsync(int id, string newRefresh);
}

