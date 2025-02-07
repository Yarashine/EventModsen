﻿using Domain.Entities;

namespace Domain.Interfaces;

public interface IImageRepository
{
    public Task AddImageUrlToEvent(int eventId, string imageUrl, CancellationToken cancelToken = default);
    public Task RemoveAllImagesFromEvent(int eventId, CancellationToken cancelToken = default);
    public Task<IEnumerable<ImageInfo>> GetAllImageUrlsFromEvent(int eventId, CancellationToken cancelToken = default);
}
