using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;

namespace Application.UseCases.Members.Commands.AddMemberToEvent;


public class AddMemberToEventCommandHandler(IMemberRepository _memberRepository, IEventRepository _eventRepository) : IRequestHandler<AddMemberToEventCommand>
{

    public async Task Handle(AddMemberToEventCommand request, CancellationToken cancellationToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(request.EventId, cancellationToken) ?? throw new NotFoundException("Event");
        var members = await _memberRepository.GetAllByEventIdAsync(request.EventId, cancellationToken);

        if (members is not null && members.Any(m => m.Id == request.MemberId))
            throw new AlreadyExistsException("User already participating in the event");

        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken) ?? throw new NotFoundException("Member");

        if (@event.MaxCountMembers <= members.Count())
            throw new BadRequestException("Event is full");

        await _memberRepository.AddToEventAsync(request.MemberId, request.EventId, cancellationToken);
    }

}

