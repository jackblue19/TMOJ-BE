using System;

namespace WebAPI.Models.Auth;

// Request models
public record CreateAccountRequest(
    string AccountName ,
    string AccountEmail ,
    string Password ,
    int AccountRole);

public record GoogleLoginRequest(string TokenId);

public record LoginRequest(string Identifier, string Password);
