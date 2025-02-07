namespace Infrastructure.Repositories;


using Domain.Entities;
using Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

public class MemberRepository(EventDBContext _eventDBContext) : IMemberRepository
{
    public readonly DbSet<Member> _members = _eventDBContext.Set<Member>();

    public async Task AddAsync(Member member, CancellationToken cancelToken = default)
    {
        _members.Add(member);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
    public async Task UpdateRefreshAsync(int id, string refresh, CancellationToken cancelToken = default)
    {
        var member = await _members.FirstOrDefaultAsync(m => m.Id == id, cancelToken);
        member.RefreshToken = refresh;
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
    public async Task AddToEventAsync(int memberId, int eventId, CancellationToken cancelToken = default)
    {
        var member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId, cancelToken);
        var @event = await _eventDBContext.Events.FirstOrDefaultAsync(e => e.Id == eventId, cancelToken);
        member.Events.Add(@event);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    } 

    public async Task DeleteFromEventAsync(int memberId, int eventId, CancellationToken cancelToken = default)
    {
        Member member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId, cancelToken);
        var @event = member.Events.FirstOrDefault(m => m.Id == eventId);
        member.Events.Remove(@event);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }

    public async Task<IEnumerable<Member>> GetAllByEventIdAsync(int eventId, CancellationToken cancelToken = default)
    {
        Event @event = await _eventDBContext.Events.AsNoTracking().Include(e => e.Members).FirstOrDefaultAsync(e => e.Id == eventId, cancelToken);
        return @event?.Members;
    }

    public async Task<Member> GetByIdAsync(int id, CancellationToken cancelToken = default)
    {
        return await _members.FirstOrDefaultAsync(m => m.Id == id, cancelToken);
    }

    public async Task<Member> GetByEmailAsync(string email, CancellationToken cancelToken = default)
    {
        return await _members.FirstOrDefaultAsync(m => m.Email == email, cancelToken);
    }

    public async Task ChangeRole(int memberId, Role role, CancellationToken cancelToken = default) 
    { 
        var member = await _members.FirstOrDefaultAsync(m => m.Id == memberId, cancelToken);
        member.Role = role;
        _eventDBContext.Entry(member).State = EntityState.Modified;
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
}
