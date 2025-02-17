using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Images.Commands.RemoveAllEventImages;

public record RemoveAllEventImagesCommand(int EventId) : IRequest;

