namespace Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public int MemberId { get; set; }
    public Member Member { get; set; }
    public string Message { get; set; } 
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
