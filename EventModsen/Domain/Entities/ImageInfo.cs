using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventModsen.Domain.Entities;

public class ImageInfo
{
    public int Id { get; set; }

    public string ImageUrl { get; set; }

    public int EventId { get; set; }

    public Event Event { get; set; }
}
