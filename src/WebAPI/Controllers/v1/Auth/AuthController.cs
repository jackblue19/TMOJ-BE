using Application.UseCases.Auth;
using Ardalis.Specification;
using Asp.Versioning;
using Infrastructure.Configurations.Auth;
using Infrastructure.Persistence.Scaffolded.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Principal;
using Google.Apis.Auth;
//using Infrastructure.Persistence.Scaffolded.Entities;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Auth;

namespace WebAPI.Controllers.v1.Auth;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    //private readonly IPasswordHasher _passwordHasher;
    private readonly JwtOptions _jwt;
    private readonly GoogleOptions _google;
    private readonly TmojDbContext _db;

    public AuthController(
        ITokenService tokenService ,
        IRefreshTokenService refreshTokenService ,
        //IPasswordHasher passwordHasher ,
        IOptions<JwtOptions> jwt ,
        IOptions<GoogleOptions> google ,
        TmojDbContext db)
    {
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        //_passwordHasher = passwordHasher;
        _jwt = jwt.Value;
        _google = google.Value;
        _db = db;
    }

    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("pong");


    [AllowAnonymous]
    [HttpPost("create-account")]        //  có thể dùng như này hoặc dùng theo chuẩn restful-api    ;   rcm dùng như này để sau này dễ dò ra uc nào còn thiếu :>
    public async Task<IActionResult> CreateAccount(
    [FromBody] CreateAccountRequest req ,
    CancellationToken ct)
    {
        return Ok("sample thôi đó mn tự test logic");
    }

    [AllowAnonymous]
    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest req , CancellationToken ct)
    {
        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(req.TokenId , new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _google.ClientId }
            });

            var user = await _db.Users
                .Include(u => u.UserProviders)
                .Include(u => u.UserRoleUsers).ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == payload.Email , ct);

            if ( user == null )
            {
                var studentRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleCode == "student" , ct);

                var user = new User
                    {
                        Email = payload.Email,
                        FirstName = payload.GivenName ?? "",
                        LastName = payload.FamilyName ?? "",
                        DisplayName = payload.Name,
                        AvatarUrl = payload.Picture,
                        Username = payload.Email.Split('@')[0] + Guid.NewGuid().ToString("N").Substring(0, 4),
                        EmailVerified = payload.EmailVerified,
                        LanguagePreference = "vi",
                        Status = true,
                        UserProviders = new List<UserProvider>(),
                        UserRoleUsers = new List<UserRole>()
                    };

                var provider = await _db.Providers
                    .FirstOrDefaultAsync(p => p.ProviderCode == "google", ct);

                if (provider == null)
                {
                    provider = new Provider
                    {
                        ProviderCode = "google",
                        ProviderDisplayName = "Google",
                        Enabled = true
                    };

                    _db.Providers.Add(provider);
                }

                user.UserProviders.Add(new UserProvider
                {
                    ProviderId = provider.ProviderId,
                    ProviderSubject = payload.Subject,
                    ProviderEmail = payload.Email,
                });

                if ( studentRole != null )
                {
                    if (studentRole != null)
                    {
                        user.UserRoleUsers.Add(new UserRole
                        {
                            UserId = user.UserId,
                            RoleId = studentRole.RoleId,
                        });
                    }
                }

                _db.Users.Add(user);
                await _db.SaveChangesAsync(ct);
            }

            // Create Session
            var session = new UserSession
            {
                UserId = user.UserId ,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ,
                UserAgent = Request.Headers["User-Agent"]
            };

            var refreshTokenStr = _refreshTokenService.GenerateToken();
            var refreshTokenHash = _refreshTokenService.HashToken(refreshTokenStr);

            var refreshToken = new RefreshToken
            {
                SessionId = session.SessionId ,
                TokenHash = refreshTokenHash ,
                ExpireAt = DateTime.UtcNow.AddDays(_jwt.RefreshTokenDays)
            };

            _db.UserSessions.Add(session);
            _db.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync(ct);

            var roles = user.UserRoleUsers.Select(ur => ur.Role.RoleCode).ToList();
            if ( !roles.Any() ) roles.Add("user"); // Default role

            var accessToken = _tokenService.CreateAccessToken(
                user.UserId.ToString() ,
                user.DisplayName ?? user.Username ,
                roles);

            var userDto = new UserDto(
                UserId: user.UserId ,
                Email: user.Email ,
                FirstName: user.FirstName ,
                LastName: user.LastName ,
                DisplayName: user.DisplayName ,
                Username: user.Username ,
                AvatarUrl: user.AvatarUrl ,
                Roles: roles
            );

            return Ok(new AuthRespone(
                AccessToken: accessToken ,
                RefreshToken: refreshTokenStr ,
                ExpiresIn: _jwt.AccessTokenMinutes * 60 ,
                User: userDto
            ));
        }
        catch ( InvalidJwtException )
        {
            return BadRequest("Invalid Google Token");
        }
        catch ( Exception ex )
        {
            return StatusCode(500 , "Internal Server Error during Google Login");
        }
    }
}
