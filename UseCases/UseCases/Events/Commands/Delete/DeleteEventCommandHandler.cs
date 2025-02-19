
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;
using Application.UseCases.Images.Commands.RemoveAllEventImages;

namespace Application.UseCases.Events.Commands.Delete;

public class DeleteEventCommandHandler(IEventRepository _eventRepository, IMediator _mediator) : IRequestHandler<DeleteEventCommand>
{
    public async Task Handle(DeleteEventCommand request, CancellationToken cancelToken = default)
    {
        var @event = await _eventRepository.GetByIdAsync(request.Id, cancelToken) ?? throw new NotFoundException("Event");
        await _mediator.Send(new RemoveAllEventImagesCommand(request.Id), cancelToken);
        await _eventRepository.DeleteAsync(request.Id, cancelToken);
    }
}
