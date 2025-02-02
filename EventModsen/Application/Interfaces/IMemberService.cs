namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

public interface IMemberService
{
    public Task AddToEvent(int memberId, int eventId);
    public Task<IEnumerable<MemberDto>> GetAllMembersByEvent(int eventId);
    public Task<MemberDto> GetMemberById(int id);
    public Task DeleteMemberFromEvent(int memberId, int eventId);

}
