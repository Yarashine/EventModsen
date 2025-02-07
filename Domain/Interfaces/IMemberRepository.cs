namespace Domain.Interfaces;
using Domain.Entities;

public interface IMemberRepository
{
    public Task AddToEventAsync(int memberId, int eventId, CancellationToken cancelToken = default);
    public Task<IEnumerable<Member>> GetAllByEventIdAsync(int eventId, CancellationToken cancelToken = default);
    public Task<Member> GetByIdAsync(int id, CancellationToken cancelToken = default);
    public Task<Member> GetByEmailAsync(string email, CancellationToken cancelToken = default);
    public Task DeleteFromEventAsync(int memberId, int eventId, CancellationToken cancelToken = default);
    public Task AddAsync (Member member, CancellationToken cancelToken = default);
    public Task UpdateRefreshAsync(int id, string newRefresh, CancellationToken cancelToken = default);
    public Task ChangeRole(int memberId, Role role, CancellationToken cancelToken = default);
}

