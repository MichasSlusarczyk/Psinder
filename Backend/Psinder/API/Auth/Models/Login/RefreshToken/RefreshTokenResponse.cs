namespace Psinder.API.Auth.Models.Logins;

public class RefreshTokenResponse
{
    public RefreshTokenResponse(RefreshTokenStatus AuthStatus, string? loginToken = null, string? refreshToken = null)
    {
        Status = AuthStatus;
        AccessToken = loginToken;
        RefreshToken = refreshToken;
    }

    public RefreshTokenStatus Status { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}
