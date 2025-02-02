using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace EventModsen.Domain.Entities;

public class Member
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }

    [DefaultValue("GETUTCDATE()")]
    public DateTime DateOfRegistration { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string Email { get; set; }
    public ICollection<Event>? Events { get; set; }

    [MaxLength(256)]
    public string PasswordHash { get; set; }

    [MaxLength(256)]
    public string PasswordSalt { get; set; }

    public Role Role { get; set; }

    public string RefreshToken { get; set; }
}

