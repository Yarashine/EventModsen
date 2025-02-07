namespace EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Application.DTOs;

public interface IMemberService
{
    public Task AddToEvent(int memberId, int eventId, CancellationToken cancelToken = default);
    public Task<IEnumerable<MemberDto>> GetAllMembersByEvent(int eventId, CancellationToken cancelToken = default);
    public Task<MemberDto> GetMemberById(int id, CancellationToken cancelToken = default);
    public Task DeleteMemberFromEvent(int memberId, int eventId, CancellationToken cancelToken = default);

}
