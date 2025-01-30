using Microsoft.EntityFrameworkCore;
using EventModsen.Domain.Entities;

namespace EventModsen.Infrastructure.DB;

public class EventDBContext : DbContext
{
    public EventDBContext(DbContextOptions<EventDBContext> options) : base(options)
    { 
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Event> Events { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Event>()
            .HasIndex(e => e.Name)
            .IsUnique();


    }
}
