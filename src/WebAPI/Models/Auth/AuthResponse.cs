namespace WebAPI.Models.Auth;

public record UserDto(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    string? DisplayName,
    string? Username,
    string? AvatarUrl,
    List<string> Roles);

public record AuthRespone(
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    UserDto User);

