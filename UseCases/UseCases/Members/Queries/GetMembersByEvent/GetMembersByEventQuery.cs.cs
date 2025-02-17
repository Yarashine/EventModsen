using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Members.Queries.GetMemberByEvent;

public record GetMembersByEventQuery(int EventId) : IRequest<IEnumerable<MemberDto>>;
