namespace Infrastructure.Services.Authentication;

using Application.Contracts;
using System.Security.Cryptography;
using System.Text;

public class AuthService() : IAuthService
{
    public int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth > today.AddYears(-age))
            age--;
        return age;
    }

    public void CreatePasswordHashAndSalt(string password, out string passwordHash, out string passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = Convert.ToBase64String(hmac.Key);
        passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
    }
    public bool CheckPassword(string password, string userPasswordHash, string userPasswordSalt)
    {
        using var hmac = new HMACSHA512(Convert.FromBase64String(userPasswordSalt));
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(Convert.FromBase64String(userPasswordHash));
    }
}
