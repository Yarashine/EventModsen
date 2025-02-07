using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface INotificationUseCase
{
    public Task AddNotification(string message, int memberId, CancellationToken cancelToken = default);
    public Task<IEnumerable<NotificationDto>> GetAllMembersUnreadNotifications(int memberId, CancellationToken cancelToken = default);
}
