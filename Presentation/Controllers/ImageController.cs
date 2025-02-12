﻿using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;


namespace Presentation.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController(IImageUseCase _imageService) : Controller
{
    [HttpGet("{eventId}")]
    public async Task<ActionResult<IEnumerable<ImageInfoDto>>> GetAllByEventImages([FromRoute]int eventId, CancellationToken cancelToken = default)
    {
        var images = await _imageService.GetAllEventImages(eventId, cancelToken);
        return Ok(images);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{eventId}")]
    public async Task<IActionResult> AddImage([FromRoute]int eventId, IFormFile image, CancellationToken cancelToken = default)
    {
        await _imageService.SaveImageAsync(image, eventId, cancelToken);
        return Ok();
    }
}
