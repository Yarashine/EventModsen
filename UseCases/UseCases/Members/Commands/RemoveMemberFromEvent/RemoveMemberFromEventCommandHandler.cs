using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;

namespace Application.UseCases.Members.Commands.RemoveMemberFromEvent;


public class RemoveMemberFromEventCommandHandler(IMemberRepository _memberRepository, IEventRepository _eventRepository) : IRequestHandler<RemoveMemberFromEventCommand>
{

    public async Task Handle(RemoveMemberFromEventCommand request, CancellationToken cancellationToken)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(request.EventId, cancellationToken);

        if (members is null || !members.Any(m => m.Id == request.MemberId))
            throw new BadRequestException("User isn't participating in the event");

        await _memberRepository.DeleteFromEventAsync(request.MemberId, request.EventId, cancellationToken);
    }
}

