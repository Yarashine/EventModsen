using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts;

public interface IImageService
{
    public Task<string> SaveImage(IFormFile image, int eventId, CancellationToken cancelToken);
    public void RemoveAllImageFilesFromEvent(int eventId);
}
