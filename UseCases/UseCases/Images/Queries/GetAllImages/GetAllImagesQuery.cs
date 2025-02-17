using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Images.Queries.GetAllImages;

public record GetAllImagesQuery(int EventId) : IRequest<IEnumerable<ImageInfoDto>>;
