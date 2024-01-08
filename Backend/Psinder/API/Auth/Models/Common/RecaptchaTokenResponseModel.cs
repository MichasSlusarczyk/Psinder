namespace Psinder.API.Auth.Models.Common;

public class RecaptchaTokenResponseModel
{
    public bool Success { get; set; }

    public string Hostname { get; set; }

    public DateTime ChallengeTs { get; set; }
}