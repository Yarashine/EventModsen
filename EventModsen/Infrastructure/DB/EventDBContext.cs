using Microsoft.EntityFrameworkCore;
using EventModsen.Domain.Entities;
using EventModsen.Infrastructure.DB.Configurations;

namespace EventModsen.Infrastructure.DB;

public class EventDBContext : DbContext
{
    public EventDBContext(DbContextOptions<EventDBContext> options) : base(options)
    { 
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ImageInfo> ImageInfos { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new MemberConfiguration());
        modelBuilder.ApplyConfiguration(new ImageInfoConfiguration());



    }
}
