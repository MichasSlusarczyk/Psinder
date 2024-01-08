namespace Psinder.API.Common.Models.Translate;

public class TranslateApiResponse
{
    public string TranslatedText { get; set; }

    public TranslateApiDetectedLanguageResponse DetectedLanguage { get; set; }

    public class TranslateApiDetectedLanguageResponse
    {
        public string Language { get; set; }
        public int Confidence { get; set; }
    }
}