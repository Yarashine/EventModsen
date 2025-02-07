using EventModsen.Application.DTOs;

namespace EventModsen.Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file, int eventId, CancellationToken cancelToken = default);
        public Task RemoveAllEventImages(int eventId, CancellationToken cancelToken = default);
        public Task<IEnumerable<ImageInfoDto>> GetAllEventImages(int eventId, CancellationToken cancelToken = default);

    }
}
