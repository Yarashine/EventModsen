namespace Application.DTOs;

public class EventDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateTimeEvent { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxCountMembers { get; set; }
}
