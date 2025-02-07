using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Id)
            .ValueGeneratedOnAdd();

        builder.Property(n => n.Message)
            .HasMaxLength(500)
            .IsRequired();

        builder.HasOne(n => n.Member)
            .WithMany(m => m.Notifications)
            .HasForeignKey(n => n.MemberId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
