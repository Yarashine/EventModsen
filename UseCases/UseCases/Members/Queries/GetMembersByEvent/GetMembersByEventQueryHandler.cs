using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Members.Queries.GetMemberByEvent;


public class GetMembersByEventQueryHandler(IMemberRepository _memberRepository, IEventRepository _eventRepository) : IRequestHandler<GetMembersByEventQuery, IEnumerable<MemberDto>>
{

    public async Task<IEnumerable<MemberDto>> Handle(GetMembersByEventQuery request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(request.EventId, cancellationToken);
        return members.Adapt<IEnumerable<MemberDto>>();
    }
}

