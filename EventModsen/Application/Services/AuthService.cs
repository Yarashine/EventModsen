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

public class AuthService(IMemberRepository _memberRepository, IJwtService _jwtService)  : IAuthService
{
    public async Task<AuthResponseDto> Register(RegisterDto member)
    {
        var checkEmailMember = await _memberRepository.GetByEmailAsync(member.Email);
        if (checkEmailMember is not null)
            throw new Exception("User with this email already exist");


        var entity = member.Adapt<Member>();
        await _memberRepository.AddAsync(entity);

        CreatePasswordHashAndSalt(member.Password, out var passwordHash, out var passwordSalt);
        entity.PasswordHash = passwordHash;
        entity.PasswordSalt = passwordSalt;

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(entity.Id, "User", age);
        var refreshToken = _jwtService.GenerateRefreshToken(entity.Id, "User", age);

        await _memberRepository.UpdateRefreshAsync(entity.Id, refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
    public async Task<AuthResponseDto> Login(LoginDto credentials)
    {
        var member = await _memberRepository.GetByEmailAsync(credentials.Email);
        if (member is null)
            throw new Exception("Invalid credentials");

        if(!CheckPassword(credentials.Password, member.PasswordHash, member.PasswordSalt))
            throw new Exception("Invalid credentials");

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(member.Id, member.Role.ToString(), age);
        var refreshToken = _jwtService.GenerateRefreshToken(member.Id, member.Role.ToString(), age);

        await _memberRepository.UpdateRefreshAsync(member.Id, refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

    }

    public async Task<AuthResponseDto> GetNewAccessToken(string oldRefreshToken)
    {
        var id = _jwtService.GetUserIdFromToken(oldRefreshToken) ?? -1;

        if (id < 0)
            return null;

        var member = await _memberRepository.GetByIdAsync(id);

        if (member is null)
            throw new Exception("User with this id doesn't exist");

        if (member.RefreshToken != oldRefreshToken)
            throw new Exception("Invalid token");

        var age = CalculateAge(member.DateOfBirth);

        var accessToken = _jwtService.GenerateAccessToken(id, "User", age);
        var refreshToken = _jwtService.GenerateRefreshToken(id, "User", age);

        await _memberRepository.UpdateRefreshAsync(id, refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    public async Task LogOut(int memberId)
    {
        await _memberRepository.UpdateRefreshAsync(memberId, null);
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
