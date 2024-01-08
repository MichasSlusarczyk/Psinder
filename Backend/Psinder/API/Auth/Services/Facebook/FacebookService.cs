using Psinder.API.Auth.Models.Common;
using Psinder.API.Common.Services;
using System.Text.Json;

namespace Psinder.API.Auth.Services.Facebook;

public class FacebookService : IFacebookService
{
    private readonly IConfiguration _configuration;
    private static string FacebookApiId { get; set; }
    private static string FacebookAppSecret { get; set; }
    private static string FacebookVerfiyAPIAddress { get; set; }
    private static string FacebookGetDataAPIAddress { get; set; }


    public FacebookService(
        IConfiguration configuration)
    {
        _configuration = configuration;

        FacebookApiId = _configuration.GetSection("Facebook:AppId").Value 
            ?? throw new Exception("Configuration value not found: Facebook:AppId");
        FacebookAppSecret = _configuration.GetSection("Facebook:AppSecret").Value
                        ?? throw new Exception("Facebook:AppSecret");
        FacebookVerfiyAPIAddress = _configuration.GetSection("Facebook:VerfiyAPIAddress").Value
                ?? throw new Exception("Facebook:VerfiyAPIAddress");
        FacebookGetDataAPIAddress = _configuration.GetSection("Facebook:GetDataAPIAddress").Value
                ?? throw new Exception("Facebook:GetDataAPIAddress");
    }

    public async Task<bool> VerifyAsync(string token, CancellationToken cancellationToken)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync($"{FacebookVerfiyAPIAddress}?input_token={token}&access_token={FacebookApiId}|{FacebookAppSecret}", cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
            };

            var tokenResponse = JsonSerializer.Deserialize<FacebookDebugTokenResponseModel>(response, options);
            if (!tokenResponse.Data.IsValid)
            {
                return false;
            }
        }

        return true;
    }

    public async Task<string> GetDataAsync(string token, CancellationToken cancellationToken)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync($"{FacebookGetDataAPIAddress}?fields=email&access_token={token}", cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
            };

            return JsonSerializer.Deserialize<FacebookUserDataResponseModel>(response, options).Email;
        }
    }
}