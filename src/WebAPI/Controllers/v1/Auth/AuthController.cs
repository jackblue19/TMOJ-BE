using Application.UseCases.Auth;
using Ardalis.Specification;
using Asp.Versioning;
using Infrastructure.Configurations.Auth;
using Infrastructure.Persistence.Scaffolded.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Principal;

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
    private readonly TmojDbContext _db;

    public AuthController(
        ITokenService tokenService ,
        IRefreshTokenService refreshTokenService ,
        //IPasswordHasher passwordHasher ,
        IOptions<JwtOptions> jwt ,
        TmojDbContext db)
    {
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        //_passwordHasher = passwordHasher;
        _jwt = jwt.Value;
        _db = db;
    }

    [AllowAnonymous]
    [HttpGet("ping")]
    public IActionResult Ping() => Ok("pong");


    public sealed record CreateAccountRequest(
        string AccountName ,
        string AccountEmail ,
        string Password ,
        int AccountRole);

    [AllowAnonymous]
    [HttpPost("create-account")]        //  có thể dùng như này hoặc dùng theo chuẩn restful-api    ;   rcm dùng như này để sau này dễ dò ra uc nào còn thiếu :>
    public async Task<IActionResult> CreateAccount(
    [FromBody] CreateAccountRequest req ,
    CancellationToken ct)
    {
        return Ok("sample thôi đó mn tự test logic");
    }


}
