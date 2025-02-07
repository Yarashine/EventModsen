namespace Infrastructure.Configurations;

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ImageInfoConfiguration : IEntityTypeConfiguration<ImageInfo>
{
    public void Configure(EntityTypeBuilder<ImageInfo> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd();

        builder.Property(i => i.ImageUrl)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasOne(i => i.Event)
            .WithMany(e => e.Images)
            .HasForeignKey(i => i.EventId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}
