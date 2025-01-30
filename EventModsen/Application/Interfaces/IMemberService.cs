namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

public interface IMemberService
{
    public Task<bool> AddToEvent(int memberId, int eventId);
    public Task<IEnumerable<MemberDto>?> GetAllMembersByEvent(int eventId);
    public Task<MemberDto?> GetMemberById(int id);
    public Task<bool> DeleteMemberFromEvent(int memberId, int eventId);

}
