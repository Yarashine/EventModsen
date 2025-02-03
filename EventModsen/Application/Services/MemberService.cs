namespace EventModsen.Application.Services;

using EventModsen.Application.DTOs;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using Mapster;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Domain.Exceptions;

public class MemberService(IMemberRepository _memberRepository, IEventRepository _eventRepository) : IMemberService
{
    public async Task AddToEvent(int memberId, int eventId)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId);
        if (members is not null && members.Any(m => m.Id == memberId))
            throw new BadRequestException("User already participating in the event");
        var member = await _memberRepository.GetByIdAsync(memberId) ?? throw new NotFoundException("Member");
        await _memberRepository.AddToEventAsync(memberId, eventId);
    }

    public async Task DeleteMemberFromEvent(int memberId, int eventId)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId);
        if (members is null || !members.Any(m => m.Id == memberId))
            throw new BadRequestException("User isn't participating in the event");
        await _memberRepository.DeleteFromEventAsync(memberId, eventId);
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersByEvent(int eventId)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId);
        return members.Adapt<IEnumerable<MemberDto>>();
    }

    public async Task<MemberDto> GetMemberById(int memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId) ?? throw new NotFoundException("Member");
        return member.Adapt<MemberDto>();
    }


}
