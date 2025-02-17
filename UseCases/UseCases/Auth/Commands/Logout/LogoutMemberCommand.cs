using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth.Commands.Logout;

public class LogoutMemberCommand : IRequest
{
    public int MemberId { get; set; }

    public LogoutMemberCommand(int memberId)
    {
        MemberId = memberId;
    }
}
