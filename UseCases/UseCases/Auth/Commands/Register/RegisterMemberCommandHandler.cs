using Application.Boundaries;
using Application.DTOs.Response;
using Domain.Entities;
using Domain.Exceptions;
using Application.RepositoryInterfaces;
using Mapster;
using MediatR;

namespace Application.UseCases.Auth.Commands.RegisterMember;

public class RegisterCommandHandler(IMemberRepository _memberRepository, IJwtService _jwtUseCase, IAuthService _authUseCase) : IRequestHandler<RegisterMemberCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(RegisterMemberCommand request, CancellationToken cancellationToken)
    {
        var member = request.Member;
        var checkEmailMember = await _memberRepository.GetByEmailAsync(member.Email, cancellationToken);
        if (checkEmailMember != null)
            throw new BadRequestException("User with this email already exists");

        var entity = member.Adapt<Member>();
        _authUseCase.CreatePasswordHashAndSalt(member.Password, out var passwordHash, out var passwordSalt);
        entity.PasswordHash = passwordHash;
        entity.PasswordSalt = passwordSalt;

        await _memberRepository.AddAsync(entity, cancellationToken);

        var age = _authUseCase.CalculateAge(member.DateOfBirth);
        var accessToken = _jwtUseCase.GenerateAccessToken(entity.Id, "User", age, cancellationToken);
        var refreshToken = _jwtUseCase.GenerateRefreshToken(entity.Id, "User", age, cancellationToken);

        await _memberRepository.UpdateRefreshAsync(entity.Id, refreshToken, cancellationToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

