using Application.DTOs.RequestDto;
using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth.Commands.RegisterMember;

public class RegisterMemberCommand : IRequest<AuthResponseDto>
{
    public RegisterDto Member { get; set; }

    public RegisterMemberCommand(RegisterDto member)
    {
        Member = member;
    }
}
