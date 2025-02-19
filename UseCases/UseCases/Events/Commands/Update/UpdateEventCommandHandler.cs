
using Domain.Entities;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;
using Application.UseCases.Notifications.Commands.Create;

namespace Application.UseCases.Events.Commands.Update;

public class UpdateEventCommandHandler(IEventRepository _eventRepository, IMemberRepository _memberRepository, IMediator _mediator) : IRequestHandler<UpdateEventCommand>
{
    public async Task Handle(UpdateEventCommand request, CancellationToken cancelToken = default)
    {
        var eventForCheckingName = await _eventRepository.GetByIdAsync(request.Id, cancelToken) ?? throw new NotFoundException("Event");
        if (eventForCheckingName.Name != request.Name)
        {
            var eventWithName = await _eventRepository.GetByNameAsync(request.Name, cancelToken);
            if (eventWithName is not null)
                throw new AlreadyExistsException("Event with this name already exists");
        }

        if (eventForCheckingName.Location != request.Location)
        {
            var eventMembers = await _memberRepository.GetAllByEventIdAsync(request.Id, cancelToken);
            foreach (var member in eventMembers)
            {
                await _mediator.Send(new CreateNotificationCommand($"The location has changed to {request.Location}", member.Id), cancelToken);
            }
        }
        else if (eventForCheckingName.DateTimeEvent != request.DateTimeEvent)
        {
            var eventMembers = await _memberRepository.GetAllByEventIdAsync(request.Id, cancelToken);
            foreach (var member in eventMembers)
            {
                await _mediator.Send(new CreateNotificationCommand($"The date and time has changed to {request.DateTimeEvent}", member.Id), cancelToken);
            }
        }

        var eventEntity = request.Adapt<Event>();
        await _eventRepository.UpdateAsync(eventEntity, cancelToken);
    }
}
