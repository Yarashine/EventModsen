﻿namespace Infrastructure.Repositories;
using Application.RepositoryInterfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Azure.Core;
using Domain.Exceptions;
using System.Threading;
using Application.Configuration;
using Microsoft.Extensions.Options;
using MediatR;

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
