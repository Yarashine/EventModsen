using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventModsen.Infrastructure.DB.Repositories;

public class NotificationRepository(EventDBContext _eventDBContext) : INotificationRepository
{
    public readonly DbSet<Notification> _notifications = _eventDBContext.Notifications;
    public async Task AddNotificationAsync(Notification notification, CancellationToken cancelToken = default)
    {
        await _notifications.AddAsync(notification, cancelToken);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId, CancellationToken cancelToken = default)
    {
        return await _notifications.AsNoTracking().Where(n => n.MemberId == userId && !n.IsRead).ToListAsync(cancelToken);
    }

    public async Task MarkAsReadAsync(int notificationId, CancellationToken cancelToken = default)
    {
        var notification = await _notifications.FirstOrDefaultAsync(notification => notification.Id == notificationId, cancelToken);
        notification.IsRead = true;
        _eventDBContext.Entry(notification).State = EntityState.Modified;
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }

    public async Task<Notification> GetByIdAsync(int id, CancellationToken cancelToken = default)
    {
        return await _notifications.FirstOrDefaultAsync(n => n.Id == id, cancelToken);
    }
}
