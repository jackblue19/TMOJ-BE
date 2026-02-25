using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations.Auth;

public interface IRefreshTokenService
{
    string GenerateToken();
    string HashToken(string token);
}

public sealed class RefreshTokenService : IRefreshTokenService
{
    public string GenerateToken()
    {
        // 64 bytes random -> base64url-ish
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes)
            .Replace("+" , "-")
            .Replace("/" , "_")
            .TrimEnd('=');
    }

    public string HashToken(string token)
    {
        // SHA256(token)
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash); // uppercase hex
    }
}