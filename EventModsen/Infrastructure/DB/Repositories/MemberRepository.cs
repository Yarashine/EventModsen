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
    public async Task<bool> AddToEventAsync(int memberId, int eventId)
    {
        var member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId);
        if (member is null)
            return false;
        var @event = member.Events.FirstOrDefault(e => e.Id == eventId);
        if (@event is null)
            return false;
        member.Events.Add(@event);
        return await _eventDBContext.SaveChangesAsync() > 0;

    } // подумать над тем чтобы использовать .? чтобы не писать проверку на Null

    public async Task<bool> DeleteFromEventAsync(int memberId, int eventId)
    {
        Member? member = await _members.Include(m => m.Events).FirstOrDefaultAsync(m => m.Id == memberId);
        if (member is not null)
        {
            var @event = member.Events.FirstOrDefault(m => m.Id == eventId);
            if (@event is null)
                return false;
            member.Events.Remove(@event);
            return await _eventDBContext.SaveChangesAsync() > 0;
        }
        return false;
    }

    public async Task<IEnumerable<Member>?> GetAllByEventIdAsync(int eventId)
    {
        Event? @event = await _eventDBContext.Events.Include(e => e.Members).FirstOrDefaultAsync(e => e.Id == eventId);
        return @event?.Members;
    }

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _members.FirstOrDefaultAsync(m => m.Id == id);
    }

}
