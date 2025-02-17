using Application.DTOs.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth.Commands.GetNewAccessToken;

public class GetNewAccessTokenCommand : IRequest<AuthResponseDto>
{
    public string OldRefreshToken { get; set; }

    public GetNewAccessTokenCommand(string oldRefreshToken)
    {
        OldRefreshToken = oldRefreshToken;
    }
}

