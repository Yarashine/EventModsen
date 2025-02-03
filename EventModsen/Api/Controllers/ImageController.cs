using Microsoft.AspNetCore.Mvc;
using EventModsen.Application.Interfaces;
using EventModsen.Application.DTOs;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;


namespace EventModsen.Api.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController(IImageService _imageService) : Controller
{
    [HttpGet("{eventId}")]
    public async Task<ActionResult<IEnumerable<ImageInfoDto>>> GetAllByEventImages([FromRoute]int eventId)
    {
        var images = await _imageService.GetAllEventImages(eventId);
        return Ok(images);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{eventId}")]
    public async Task<IActionResult> AddImage([FromRoute]int eventId, IFormFile image)
    {
        await _imageService.SaveImageAsync(image, eventId);
        return Ok();
    }
}
