using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UseCases.Auth;

/// <summary>
/// Này là sample cho Tuấn
/// đó thích thì dựa trên base này ko thì code kiểu khác cũng được nhưng cần phải chuẩn
/// yêu cầu cần có cả access token và refresh token
/// </summary>

public interface ITokenService
{
    string CreateAccessToken(
        string userId ,
        string? userName ,
        IEnumerable<string> roles ,
        IDictionary<string , string>? extraClaims = null);
}
