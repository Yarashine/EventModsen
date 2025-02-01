using EventModsen.Domain.Entities;

namespace EventModsen.Domain.Interfaces;

public interface IImageRepository
{
    public Task AddImageUrlToEvent(int eventId, string imageUrl);
    public Task RemoveAllImagesFromEvent(int eventId);
    public Task<IEnumerable<ImageInfo>> GetAllImageUrlsFromEvent(int eventId);
}
