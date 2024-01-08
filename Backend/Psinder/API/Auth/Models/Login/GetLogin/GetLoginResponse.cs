namespace Psinder.API.Auth.Models.Logins;

public class GetAuthResponse
{
    public GetAuthResponse(string? login)
    {
        Login = login;
    }

    public string? Login { get; set; }
}
