using Psinder.API.Common.Models.Translate;

namespace Psinder.API.Common.Services;

public interface ITranslatorService
{
    Task<TranslateApiResponse> Translate(string text, LanguageKinds from, LanguageKinds to, CancellationToken cancellationToken);

}
