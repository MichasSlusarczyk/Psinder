using Psinder.API.Common.Services;

namespace Psinder.API.Domain.Models.Translate;

public class TranslateRequest
{
    public string Text { get; set; }
    public LanguageKinds Source { get; set; }
    public LanguageKinds Target { get; set; }
}
