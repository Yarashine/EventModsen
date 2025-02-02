namespace EventModsen.Infrastructure.DB.Repositories;

using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using Mapster;

public class MemberRepository(EventDBContext _eventDBContext) : IMemberRepository
{
    public readonly DbSet<Member> _members = _eventDBContext.Set<Member>();

    public async Task AddAsync(Member member)
    {
        _members.Add(member);
        await _eventDBContext.SaveChangesAsync();
    }
    public async Task UpdateRefreshAsync(int id, string refresh)
    {
        var member = await _members.FirstOrDefaultAsync(m => m.Id == id);
        member.RefreshToken = refresh;
        await _eventDBContext.SaveChangesAsync();
    }
    public async Task AddToEventAsync(int memberId, int eventId)
    {
        var member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId);
        var @event = _eventDBContext.Events.FirstOrDefault(e => e.Id == eventId);
        member.Events.Add(@event);
        await _eventDBContext.SaveChangesAsync();
    } 

    public async Task DeleteFromEventAsync(int memberId, int eventId)
    {
        Member member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId);
        var @event = member.Events.FirstOrDefault(m => m.Id == eventId);
        member.Events.Remove(@event);
        await _eventDBContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Member>> GetAllByEventIdAsync(int eventId)
    {
        Event @event = await _eventDBContext.Events.Include(e => e.Members).FirstOrDefaultAsync(e => e.Id == eventId);
        return @event?.Members;
    }

    public async Task<Member> GetByIdAsync(int id)
    {
        return await _members.FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Member> GetByEmailAsync(string email)
    {
        return await _members.FirstOrDefaultAsync(m => m.Email == email);
    }

}
