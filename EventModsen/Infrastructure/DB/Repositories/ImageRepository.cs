namespace EventModsen.Infrastructure.DB.Repositories;
using EventModsen.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventModsen.Infrastructure.DB;
using Microsoft.EntityFrameworkCore;
using EventModsen.Domain.Entities;

public class ImageRepository(EventDBContext _eventDBContext) : IImageRepository
{
    public DbSet<ImageInfo> imageInfos = _eventDBContext.ImageInfos;
    public async Task AddImageUrlToEvent(int eventId, string imageUrl)
    {
        await imageInfos.AddAsync(new ImageInfo() { ImageUrl = imageUrl, eventId = eventId });
        await _eventDBContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ImageInfo>> GetAllImageUrlsFromEvent(int eventId)
    {
        return await imageInfos.Where(i => i.eventId == eventId).ToListAsync();
    }

    public async Task RemoveAllImagesFromEvent(int eventId)
    {
        var images = imageInfos.Where(i => i.eventId == eventId);
        imageInfos.RemoveRange(images);
        await _eventDBContext.SaveChangesAsync();
    }
}
