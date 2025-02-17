using Application.DTOs.RequestDto;
using Application.DTOs.Response;
using Domain.Entities;

namespace Application.Boundaries;

public interface IAuthService
{
    public int CalculateAge(DateTime dateOfBirth);
    public void CreatePasswordHashAndSalt(string password, out string passwordHash, out string passwordSalt);
    public bool CheckPassword(string password, string userPasswordHash, string userPasswordSalt);
}
