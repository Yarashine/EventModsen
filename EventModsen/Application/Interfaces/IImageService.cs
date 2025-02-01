using EventModsen.Application.DTOs;

namespace EventModsen.Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile file, int eventId);
        public Task RemoveAllEventImages(int eventId);
        public Task<IEnumerable<ImageInfoDto>> GetAllEventImages(int eventId);

    }
}
