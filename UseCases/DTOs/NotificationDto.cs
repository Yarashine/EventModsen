using Domain.Entities;

namespace Application.DTOs;
public class NotificationDto
{
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }
}
