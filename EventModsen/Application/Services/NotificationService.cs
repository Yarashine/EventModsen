using EventModsen.Application.DTOs;
using EventModsen.Application.Interfaces;
using EventModsen.Domain.Entities;
using EventModsen.Domain.Exceptions;
using EventModsen.Domain.Interfaces;
using Mapster;

namespace EventModsen.Application.Services;

public class NotificationService(INotificationRepository _notificationRepository , IMemberRepository _memberRepository) : INotificationService
{
    public async Task AddNotification(string message, int memberId)
    {
        var notification = new Notification() { Message = message, MemberId = memberId };
        await _notificationRepository.AddNotificationAsync(notification);
    }

    public async Task<IEnumerable<NotificationDto>> GetAllMembersUnreadNotifications(int memberId)
    {
        var member = await _memberRepository.GetByIdAsync(memberId) ?? throw new NotFoundException("Member");
        var notifications = await _notificationRepository.GetUnreadNotificationsAsync(memberId);
        foreach (var notification in notifications)
        {
            await _notificationRepository.MarkAsReadAsync(notification.Id);
        }
        return notifications.Adapt<IEnumerable<NotificationDto>>();        
    }
}
