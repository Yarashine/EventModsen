using EventModsen.Domain.Entities;

namespace EventModsen.Domain.Interfaces;

public interface INotificationRepository
{
    Task AddNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId);
    Task MarkAsReadAsync(int notificationId);
    public Task<Notification> GetByIdAsync(int id);
}
