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
    public async Task AddToEvent(int memberId, int eventId, CancellationToken cancelToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId, cancelToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId, cancelToken);
        if (members is not null && members.Any(m => m.Id == memberId))
            throw new BadRequestException("User already participating in the event");
        var member = await _memberRepository.GetByIdAsync(memberId, cancelToken) ?? throw new NotFoundException("Member");
        if (@event.MaxCountMembers <= members.Count())
            throw new BadRequestException("Event is full");
        await _memberRepository.AddToEventAsync(memberId, eventId, cancelToken);
    }

    public async Task DeleteMemberFromEvent(int memberId, int eventId, CancellationToken cancelToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId, cancelToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId, cancelToken);
        if (members is null || !members.Any(m => m.Id == memberId))
            throw new BadRequestException("User isn't participating in the event");
        await _memberRepository.DeleteFromEventAsync(memberId, eventId, cancelToken);
    }

    public async Task<IEnumerable<MemberDto>> GetAllMembersByEvent(int eventId, CancellationToken cancelToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(eventId, cancelToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(eventId, cancelToken);
        return members.Adapt<IEnumerable<MemberDto>>();
    }

    public async Task<MemberDto> GetMemberById(int memberId, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(memberId, cancelToken) ?? throw new NotFoundException("Member");
        return member.Adapt<MemberDto>();
    }


}
