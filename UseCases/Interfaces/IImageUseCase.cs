using Microsoft.AspNetCore.Http;
using Application.DTOs;

namespace Application.Interfaces;

public interface IImageUseCase
{
    Task<string> SaveImageAsync(IFormFile file, int eventId, CancellationToken cancelToken = default);
    public Task RemoveAllEventImages(int eventId, CancellationToken cancelToken = default);
    public Task<IEnumerable<ImageInfoDto>> GetAllEventImages(int eventId, CancellationToken cancelToken = default);

}
