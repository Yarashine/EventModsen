using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventModsen.Domain.Entities;

public class ImageInfo
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(100)]
    public string ImageUrl { get; set; }

    public int eventId { get; set; }

    public Event @event { get; set; }
}
