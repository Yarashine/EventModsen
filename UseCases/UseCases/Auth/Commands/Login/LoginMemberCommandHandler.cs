using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Authentication;
using Application.Contracts;

namespace Application.UseCases.Auth.Commands.Login;

public class LoginCommandHandler(IMemberRepository _memberRepository, IJwtService _jwtUseCase, IAuthService _authUseCase) : IRequestHandler<LoginMemberCommand, AuthResponseDto>
{

    public async Task<AuthResponseDto> Handle(LoginMemberCommand request, CancellationToken cancellationToken)
    {
        var credentials = request.Credentials;
        var member = await _memberRepository.GetByEmailAsync(credentials.Email, cancellationToken) ?? throw new NotFoundException("Member");

        if (!_authUseCase.CheckPassword(credentials.Password, member.PasswordHash, member.PasswordSalt))
            throw new UnauthorizedAccessException("Invalid username or password.");

        var age = _authUseCase.CalculateAge(member.DateOfBirth);
        var accessToken = _jwtUseCase.GenerateAccessToken(member.Id, member.Role.ToString(), age, cancellationToken);
        var refreshToken = _jwtUseCase.GenerateRefreshToken(member.Id, member.Role.ToString(), age, cancellationToken);

        await _memberRepository.UpdateRefreshAsync(member.Id, refreshToken, cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

