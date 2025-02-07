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
    public async Task AddImageUrlToEvent(int eventId, string imageUrl, CancellationToken cancelToken = default)
    {
        await imageInfos.AddAsync(new ImageInfo() { ImageUrl = imageUrl, EventId = eventId }, cancelToken);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }

    public async Task<IEnumerable<ImageInfo>> GetAllImageUrlsFromEvent(int eventId, CancellationToken cancelToken = default)
    {
        return await imageInfos.Where(i => i.EventId == eventId).ToListAsync(cancelToken);
    }

    public async Task RemoveAllImagesFromEvent(int eventId, CancellationToken cancelToken = default)
    {
        var images = await imageInfos.Where(i => i.EventId == eventId).ToListAsync(cancelToken);
        imageInfos.RemoveRange(images);
        await _eventDBContext.SaveChangesAsync(cancelToken);
    }
}
