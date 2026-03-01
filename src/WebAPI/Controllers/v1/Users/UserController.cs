using Application.UseCases.Auth;
using Asp.Versioning;
using Domain.Entities;
using Infrastructure.Configurations.Auth;
using Infrastructure.Persistence.Scaffolded.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace WebAPI.Controllers.v1.Users;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly TmojDbContext _db;
    private readonly IPasswordHasher _passwordHasher;

    public UserController(TmojDbContext db, IPasswordHasher passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserRequest req ,
        CancellationToken ct)
    {
        var email = req.Email.ToLowerInvariant();
        if ( await _db.Users.AnyAsync(u => u.Email == email , ct) )
        {
            return BadRequest("Email already exists");
        }

        var user = new User
        {
            FirstName = req.FirstName ,
            LastName = req.LastName ,
            Email = email ,
            Password = _passwordHasher.Hash(req.Password) ,
            Username = req.Username ?? (email.Split('@')[0] + Random.Shared.Next(1000 , 9999).ToString()) ,
            DisplayName = $"{req.FirstName} {req.LastName}" ,
            LanguagePreference = "vi" ,
            Status = true ,
            EmailVerified = true // Admin created users are verified by default
        };

        if ( req.Roles != null && req.Roles.Any() )
        {
            foreach ( var roleCode in req.Roles )
            {
                var normalizedRoleCode = roleCode.ToLowerInvariant();
                var role = await _db.Roles.FirstOrDefaultAsync(r => r.RoleCode == normalizedRoleCode , ct);
                if ( role != null )
                {
                    user.UserRoleUsers.Add(new UserRole { RoleId = role.RoleId });
                }
            }
        }
        else
        {
            var studentRole = await _db.Roles.FirstOrDefaultAsync(r => r.RoleCode == "student" , ct);
            if ( studentRole != null )
            {
                user.UserRoleUsers.Add(new UserRole { RoleId = studentRole.RoleId });
            }
        }

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);

        return Ok(new { Message = "User created successfully" , UserId = user.UserId });
    }


    [HttpGet("list-all")]
    public async Task<IActionResult> ListAll(CancellationToken ct)
    {
        var users = await _db.Users
            .Where(u => u.DeletedAt == null)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserProfileResponse(
                u.UserId,
                u.Email,
                u.FirstName,
                u.LastName,
                u.DisplayName,
                u.Username,
                u.AvatarUrl,
                u.EmailVerified,
                u.Status,
                u.CreatedAt
            ))
            .ToListAsync(ct);

        return Ok(users);
    }

    [Authorize(Roles = "admin")]
    [HttpGet("list-banned")]
    public async Task<IActionResult> ListBanned(CancellationToken ct)
    {
        var users = await _db.Users
            .Where(u => u.DeletedAt == null && u.Status == false)
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => new UserProfileResponse(
                u.UserId,
                u.Email,
                u.FirstName,
                u.LastName,
                u.DisplayName,
                u.Username,
                u.AvatarUrl,
                u.EmailVerified,
                u.Status,
                u.CreatedAt
            ))
            .ToListAsync(ct);

        return Ok(users);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/lock")]
    public async Task<IActionResult> Lock(Guid id, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);
        if (user == null) return NotFound();

        user.Status = false;
        await _db.SaveChangesAsync(ct);

        return Ok("Account locked successfully.");
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}/unlock")]
    public async Task<IActionResult> Unlock(Guid id, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);
        if (user == null) return NotFound();

        user.Status = true;
        await _db.SaveChangesAsync(ct);

        return Ok("Account unlocked successfully.");
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(new object[] { id }, ct);
        if (user == null) return NotFound();

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);

        return Ok("Account deleted successfully.");
    }
}
