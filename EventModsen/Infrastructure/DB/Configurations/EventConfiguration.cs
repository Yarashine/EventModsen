using Microsoft.EntityFrameworkCore;
using EventModsen.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace EventModsen.Infrastructure.DB.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.Property(e => e.Description)
            .HasMaxLength(1000);;

        builder.Property(e => e.Location)
            .HasMaxLength(100);

        builder.Property(e => e.Category)
            .HasMaxLength(50);

        builder.HasMany(e => e.Members)
            .WithMany(m => m.Events);

        builder.HasMany(e => e.Images)
            .WithOne(i => i.Event)
            .HasForeignKey(i => i.EventId);
    }
}
