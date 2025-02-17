using Domain.Entities;

namespace Application.RepositoryInterfaces;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification, CancellationToken cancelToken = default);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId, CancellationToken cancelToken = default);
    Task MarkAsReadAsync(int notificationId, CancellationToken cancelToken = default);
    public Task<Notification> GetByIdAsync(int id, CancellationToken cancelToken = default);
}
