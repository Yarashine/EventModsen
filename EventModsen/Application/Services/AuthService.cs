namespace EventModsen.Application.Services;

using EventModsen.Application.DTOs;
using EventModsen.Application.Interfaces;
using Mapster;
using System.Security.Cryptography;
using System.Text;
using EventModsen.Domain.Entities;
using EventModsen.Domain.Interfaces;
using EventModsen.Application.DTOs.RequestDto;
using EventModsen.Application.DTOs.Response;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EventModsen.Domain.Exceptions;

public class AuthService(IMemberRepository _memberRepository, IJwtService _jwtService)  : IAuthService
{
    public async Task<AuthResponseDto> Register(RegisterDto member, CancellationToken cancelToken = default)
    {
        var checkEmailMember = await _memberRepository.GetByEmailAsync(member.Email, cancelToken);
        if (checkEmailMember is not null)
            throw new BadRequestException("User with this email already exist");


        var entity = member.Adapt<Member>();

        CreatePasswordHashAndSalt(member.Password, out var passwordHash, out var passwordSalt);
        entity.PasswordHash = passwordHash;
        entity.PasswordSalt = passwordSalt;


        await _memberRepository.AddAsync(entity, cancelToken);

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(entity.Id, "User", age, cancelToken);
        var refreshToken = _jwtService.GenerateRefreshToken(entity.Id, "User", age, cancelToken);

        await _memberRepository.UpdateRefreshAsync(entity.Id, refreshToken, cancelToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    public async Task<AuthResponseDto> Login(LoginDto credentials, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByEmailAsync(credentials.Email, cancelToken) ?? throw new NotFoundException("Member");

        if(!CheckPassword(credentials.Password, member.PasswordHash, member.PasswordSalt))
            throw new BadRequestException("Invalid credentials");

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(member.Id, member.Role.ToString(), age, cancelToken);
        var refreshToken = _jwtService.GenerateRefreshToken(member.Id, member.Role.ToString(), age, cancelToken);

        await _memberRepository.UpdateRefreshAsync(member.Id, refreshToken, cancelToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }

    public async Task<AuthResponseDto> GetNewAccessToken(string oldRefreshToken, CancellationToken cancelToken = default)
    {
        var id = _jwtService.GetUserIdFromToken(oldRefreshToken, cancelToken) ?? throw new BadRequestException("Invalid refresh token");

        var member = await _memberRepository.GetByIdAsync(id, cancelToken) ?? throw new NotFoundException("Member");

        if (member.RefreshToken != oldRefreshToken)
            throw new BadRequestException("Invalid refresh token");

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(id, "User", age, cancelToken);
        var refreshToken = _jwtService.GenerateRefreshToken(id, "User", age, cancelToken);

        await _memberRepository.UpdateRefreshAsync(id, refreshToken, cancelToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task LogOut(int memberId, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(memberId, cancelToken) ?? throw new NotFoundException("Member");
        await _memberRepository.UpdateRefreshAsync(memberId, null, cancelToken);
    }


    public async Task ChangeMemberRole(int memberId, Role role, CancellationToken cancelToken = default)
    {
        var member = await _memberRepository.GetByIdAsync(memberId, cancelToken) ?? throw new NotFoundException("Member");
        await _memberRepository.ChangeRole(memberId, role, cancelToken);
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
            age--;
        return age;
    }

    public static void CreatePasswordHashAndSalt(string password, out string passwordHash, out string passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = Convert.ToBase64String(hmac.Key);
        passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
    public static bool CheckPassword(string password, string userPasswordHash, string userPasswordSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(userPasswordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(userPasswordHash));
    }
}
