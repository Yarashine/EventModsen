namespace EventModsen.Infrastructure.DB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
public class Event
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(50)]
    public string Name { get; set; }

    [MaxLength(1000)]
    public string Description { get; set; }
    public DateTime DateTimeEvent { get; set; }

    [MaxLength(100)]
    public string Location { get; set; }

    [MaxLength(50)]
    public string Category { get; set; }

    [Range(1, 100000)]
    public int MaxCountMembers { get; set; }
    public ICollection<Member> Members { get; set; }

    [MaxLength(100)]
    public string ImagePath { get; set; }
}
