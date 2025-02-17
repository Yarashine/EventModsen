using Application.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Notifications.Commands.Create;

public class CreateNotificationCommandHandler(INotificationRepository _notificationRepository)
    : IRequestHandler<CreateNotificationCommand>
{
    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = new Notification
        {
            Message = request.Message,
            MemberId = request.MemberId
        };

        await _notificationRepository.AddNotificationAsync(notification, cancellationToken);
    }
}
