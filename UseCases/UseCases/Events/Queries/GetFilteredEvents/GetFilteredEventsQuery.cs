using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Events.Queries.GetFilteredEvents;

public record GetFilteredEventsQuery(int PageNumber, DateTime? Date, string Location, string Category) : IRequest<IEnumerable<EventDto>>;

