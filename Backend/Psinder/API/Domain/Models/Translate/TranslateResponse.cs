using Psinder.API.Common.Models.Translate;

namespace Psinder.API.Domain.Models.Translate;

public class TranslateResponse
{
    public string TranslatedText { get; set; }

    public TranslateDetectedLanguageResponse DetectedLanguage { get; set; }

    public class TranslateDetectedLanguageResponse
    {
        public string Language { get; set; }
        public int Confidence { get; set; }
    }

    public static TranslateResponse From(TranslateApiResponse apiResponse)
    {
        return new TranslateResponse()
        {
            TranslatedText = apiResponse.TranslatedText,
            DetectedLanguage = new TranslateDetectedLanguageResponse()
            {
                Confidence = apiResponse.DetectedLanguage.Confidence,
                Language = apiResponse.DetectedLanguage.Language
            }
        };
    }
}
