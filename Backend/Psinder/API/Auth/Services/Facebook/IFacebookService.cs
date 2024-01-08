namespace Psinder.API.Auth.Services.Facebook;

public interface IFacebookService
{
    Task<bool> VerifyAsync(string token, CancellationToken cancellationToken);

    Task<string> GetDataAsync(string token, CancellationToken cancellationToken);
}
