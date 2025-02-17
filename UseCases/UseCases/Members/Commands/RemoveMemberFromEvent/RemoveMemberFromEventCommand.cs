using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Members.Commands.RemoveMemberFromEvent;

public record RemoveMemberFromEventCommand(int MemberId, int EventId) : IRequest;
