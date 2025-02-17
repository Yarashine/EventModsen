using Application.DTOs.RequestDto;
using Domain.Entities;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Events.Commands.Create;

public class CreateEventCommandHandler(IEventRepository _eventRepository) : IRequestHandler<CreateEventCommand>
{

    public async Task Handle(CreateEventCommand request, CancellationToken cancelToken = default)
    {
        var eventByName = await _eventRepository.GetByNameAsync(request.Name, cancelToken);
        if (eventByName is not null)
            throw new BadRequestException("Event with this name already exist");
        var entity = request.Adapt<Event>();
        await _eventRepository.CreateAsync(entity, cancelToken);
    }

}
