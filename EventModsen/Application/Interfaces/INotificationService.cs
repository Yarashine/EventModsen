using EventModsen.Application.DTOs;
using EventModsen.Domain.Entities;

namespace EventModsen.Application.Interfaces;

public interface INotificationService
{
    public Task AddNotification(string message, int memberId, CancellationToken cancelToken = default);
    public Task<IEnumerable<NotificationDto>> GetAllMembersUnreadNotifications(int memberId, CancellationToken cancelToken = default);
}
