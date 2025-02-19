
using Application.DTOs.Response;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using MediatR;
using Application.Contracts;

namespace Application.UseCases.Auth.Commands.GetNewAccessToken;

public class GetNewAccessTokenCommandHandler(IMemberRepository _memberRepository, IJwtService _jwtUseCase, IAuthService _authUseCase) : IRequestHandler<GetNewAccessTokenCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(GetNewAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var id = _jwtUseCase.GetUserIdFromToken(request.OldRefreshToken, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid refresh token");

        var member = await _memberRepository.GetByIdAsync(id, cancellationToken) ?? throw new NotFoundException("Member");

        if (member.RefreshToken != request.OldRefreshToken)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var age = _authUseCase.CalculateAge(member.DateOfBirth);
        var accessToken = _jwtUseCase.GenerateAccessToken(id, "User", age, cancellationToken);
        var refreshToken = _jwtUseCase.GenerateRefreshToken(id, "User", age, cancellationToken);

        await _memberRepository.UpdateRefreshAsync(id, refreshToken, cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

