using Psinder.API.Common.Models.Translate;
using System.Text.Json;

namespace Psinder.API.Common.Services;

public class TranslatorService : ITranslatorService
{
    public async Task<TranslateApiResponse> Translate(string text, LanguageKinds from, LanguageKinds to, CancellationToken cancellationToken)
    {
        using (var client = new HttpClient())
        {
            var request = new TranslateApiRequest() 
            { 
                Format = "text", 
                Q = text,
                Source = ReturnLanguage(to),
                Target = ReturnLanguage(from),
                ApiKey = ""
            };
            var jsonRequest = JsonContent.Create(request);
            var response = await client.PostAsync($"https://libretranslate.com/translate", jsonRequest);
            var stringResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = new SnakeCasePropertyNamingPolicy()
            };

            return JsonSerializer.Deserialize<TranslateApiResponse>(stringResponse, options);
        }
    }

    private string ReturnLanguage(LanguageKinds language)
    {
        switch (language)
        {
            case LanguageKinds.PL:
                return "pl";
            case LanguageKinds.ENG:
                return "en";
            default:
                return "";
        }
    }
}

