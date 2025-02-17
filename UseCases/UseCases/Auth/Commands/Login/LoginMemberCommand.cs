using Application.DTOs.RequestDto;
using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth.Commands.Login;

public class LoginMemberCommand : IRequest<AuthResponseDto>
{
    public LoginDto Credentials { get; set; }

    public LoginMemberCommand(LoginDto credentials)
    {
        Credentials = credentials;
    }
}

