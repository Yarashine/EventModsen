using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Mapster;

namespace Application.Services;

public class NotificationUseCase(INotificationRepository _notificationRepository , IMemberRepository _memberRepository) : INotificationUseCase
{
    public async Task AddNotification(string message, int memberId, CancellationToken cancelToken = default)
    {
        var notification = new Notification() { Message = message, MemberId = memberId };
        await _notificationRepository.AddNotificationAsync(notification, cancelToken);
    }

    public async Task<IEnumerable<NotificationDto>> GetAllMembersUnreadNotifications(int memberId, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(memberId, cancelToken) ?? throw new NotFoundException("Member");
        var notifications = await _notificationRepository.GetUnreadNotificationsAsync(memberId, cancelToken);
        foreach (var notification in notifications)
        {
            await _notificationRepository.MarkAsReadAsync(notification.Id, cancelToken);
        }
        return notifications.Adapt<IEnumerable<NotificationDto>>();        
    }
}
