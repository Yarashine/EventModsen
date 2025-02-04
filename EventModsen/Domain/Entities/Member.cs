using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventModsen.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;
    public string Email { get; set; }
    public ICollection<Event> Events { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public Role Role { get; set; }
    public string RefreshToken { get; set; }
}

