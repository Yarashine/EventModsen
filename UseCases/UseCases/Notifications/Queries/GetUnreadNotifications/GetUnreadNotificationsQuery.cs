using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Notifications.Queries.GetUnreadNotifications;

public record GetUnreadNotificationsQuery(int MemberId) : IRequest<IEnumerable<NotificationDto>>;