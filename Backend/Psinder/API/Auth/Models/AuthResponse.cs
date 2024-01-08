namespace Psinder.API.Auth.Models;

public class AuthResponse
{
    public AuthResponse(
        AuthStatus AuthStatus, 
        string? accessToken = null, 
        string? refreshToken = null, 
        long? userId = null, 
        string? errorMessage = null)
    {
        Status = AuthStatus;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        UserId = userId;
        ErrorMessage = errorMessage;
    }

    public AuthStatus Status { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public long? UserId { get; set; }
    public string? ErrorMessage { get; set; }
}