using EventModsen.Application.DTOs;
using EventModsen.Domain.Entities;

namespace EventModsen.Application.Interfaces;

public interface INotificationService
{
    public Task AddNotification(string message, int memberId);
    public Task<IEnumerable<NotificationDto>> GetAllMembersUnreadNotifications(int memberId);
}
