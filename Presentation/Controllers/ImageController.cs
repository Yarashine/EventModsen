using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MediatR;
using Application.UseCases.Images.Queries.GetAllImages;
using Application.UseCases.Images.Commands.Create;
using Application.DTOs.Response;


namespace Presentation.Controllers;

[ApiController]
[Route("api/images")]
public class ImageController(IMediator _mediator) : Controller
{
    [HttpGet("{eventId}")]
    public async Task<ActionResult<IEnumerable<ImageInfoDto>>> GetAllByEventImages([FromRoute]int eventId, CancellationToken cancelToken = default)
    {
        var images = await _mediator.Send(new GetAllImagesQuery(eventId), cancelToken);
        return Ok(images);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{eventId}")]
    public async Task<ActionResult<string>> AddImage([FromRoute]int eventId, IFormFile image, CancellationToken cancelToken = default)
    {
        await _mediator.Send(new CreateImageCommand(eventId, image), cancelToken);
        return Ok();
    }
}
