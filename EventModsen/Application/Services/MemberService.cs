namespace EventModsen.Application.Services;

using EventModsen.Application.DTOs;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MemberService(IMemberRepository _memberRepository) : IMemberService
{
    public async Task<bool> AddToEvent(int memberId, int eventId)
    {
        return await _memberRepository.AddToEventAsync(memberId, eventId);
    }

    public async Task<bool> DeleteMemberFromEvent(int memberId, int eventId)
    {
        return await _memberRepository.DeleteFromEventAsync(memberId, eventId);
    }

    public async Task<IEnumerable<MemberDto>?> GetAllMembersByEvent(int eventId)
    {
        var member = await _memberRepository.GetAllByEventIdAsync(eventId);
        return member.Adapt<IEnumerable<MemberDto>?>();
    }

    public async Task<MemberDto?> GetMemberById(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        return member.Adapt<MemberDto>();
    }
}
