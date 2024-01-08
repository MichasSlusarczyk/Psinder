using Psinder.API.Auth.Models.Common;
using System.Text.Json;

namespace Psinder.API.Auth.Services.Recaptcha;

public class RecaptchaService : IRecaptchaService
{
    private readonly IConfiguration _configuration;
    private static string GoogleSecretKey { get; set; }
    private static string GoogleRecaptchaVerifyApi { get; set; }
    private static decimal RecaptchaThreshold { get; set; }

    public RecaptchaService(
        IConfiguration configuration)
    {
        _configuration = configuration;

        GoogleRecaptchaVerifyApi = _configuration.GetSection("Recaptcha:GoogleRecaptcha:VefiyAPIAddress").Value 
            ?? throw new Exception("Configuration value not found: Recaptcha:GoogleRecaptcha:VefiyAPIAddress");
        GoogleSecretKey = _configuration.GetSection("Recaptcha:GoogleRecaptcha:Secretkey").Value
                        ?? throw new Exception("Configuration value not found: Recaptcha:GoogleRecaptcha:Secretkey");

        var hasThresholdValue = decimal.TryParse(_configuration.GetSection("Recaptcha:RecaptchaThreshold").Value
                                    ?? throw new Exception("Configuration value not found: Recaptcha:RecaptchaThreshold"), out var threshold);    
        if (hasThresholdValue)
        {
            RecaptchaThreshold = threshold;
        }
    }

    public async Task<bool> VerifyAsync(string token)
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync($"{GoogleRecaptchaVerifyApi}?secret={GoogleSecretKey}&response={token}");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var tokenResponse = JsonSerializer.Deserialize<RecaptchaTokenResponseModel>(response, options);
            
            if (!tokenResponse.Success)
            {
                return false;
            }
        }
        return true;
    }
}