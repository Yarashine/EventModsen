using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Notifications.Queries.GetUnreadNotifications;

public class GetUnreadNotificationsQueryHandler(INotificationRepository _notificationRepository, IMemberRepository _memberRepository) : IRequestHandler<GetUnreadNotificationsQuery, IEnumerable<NotificationDto>>
{
    public async Task<IEnumerable<NotificationDto>> Handle(GetUnreadNotificationsQuery request, CancellationToken cancellationToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken) ?? throw new NotFoundException("Member");
        var notifications = await _notificationRepository.GetUnreadNotificationsAsync(request.MemberId, cancellationToken);

        foreach (var notification in notifications)
        {
            await _notificationRepository.MarkAsReadAsync(notification.Id, cancellationToken);
        }

        return notifications.Adapt<IEnumerable<NotificationDto>>();
    }
}
