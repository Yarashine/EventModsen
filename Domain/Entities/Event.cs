using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeEvent { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxCountMembers { get; set; }
    public ICollection<Member> Members { get; set; }
    public ICollection<ImageInfo> Images { get; set; }
}

