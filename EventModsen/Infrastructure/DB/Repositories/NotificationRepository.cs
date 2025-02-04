using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventModsen.Infrastructure.DB.Repositories;

public class NotificationRepository(EventDBContext _eventDBContext) : INotificationRepository
{
    public readonly DbSet<Notification> _notifications = _eventDBContext.Notifications;
    public async Task AddNotificationAsync(Notification notification)
    {
        await _notifications.AddAsync(notification);
        await _eventDBContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(int userId)
    {
        return await _notifications.AsNoTracking().Where(n => n.MemberId == userId && !n.IsRead).ToListAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _notifications.FirstOrDefaultAsync(notification => notification.Id == notificationId);
        notification.IsRead = true;
        _eventDBContext.Entry(notification).State = EntityState.Modified;
        await _eventDBContext.SaveChangesAsync();
    }

    public async Task<Notification> GetByIdAsync(int id)
    {
        return await _notifications.FirstOrDefaultAsync(n => n.Id == id);
    }
}
