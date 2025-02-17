using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Notifications.Commands.Create;

public record CreateNotificationCommand(string Message, int MemberId) : IRequest;