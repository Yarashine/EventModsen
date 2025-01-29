namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

public interface IMemberService
{
    public Task AddToEvent(int memberId, int eventId);
    public Task<List<MemberDto>?> GetAllMembersByEvent();
    public Task<MemberDto?> GetMemberById(int id);
    public Task<bool> DeleteMemberFromEvent(int memberId, int eventId);

}
