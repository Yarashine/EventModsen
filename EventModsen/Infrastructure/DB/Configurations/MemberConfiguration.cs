using EventModsen.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace EventModsen.Infrastructure.DB.Configurations;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

        builder.Property(m => m.Name)
            .HasMaxLength(50);

        builder.Property(m => m.LastName)
            .HasMaxLength(50);

        builder.Property(m => m.DateOfBirth)
            .IsRequired();

        builder.Property(m => m.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(m => m.Email)
            .IsUnique();

        builder.Property(m => m.PasswordHash)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(m => m.PasswordSalt)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(m => m.RefreshToken);

        builder.Property(m => m.Role)
            .HasConversion<string>()
            .IsRequired();

        builder.HasMany(m => m.Events)
            .WithMany(e => e.Members);

        builder.HasMany(m => m.Notifications)
            .WithOne(n => n.Member)
            .HasForeignKey(n => n.MemberId);
    }
}
