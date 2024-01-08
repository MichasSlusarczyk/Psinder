using MediatR;
using Psinder.API.Common.Services;
using Psinder.API.Domain.Models.Translate;

namespace Psinder.API.Domain.Handlers.Translate;

public class TranslateRequestHandler : IRequestHandler<TranslateMediatr, TranslateResponse>
{
    private readonly ITranslatorService _translatorService;
    private readonly ILogger<TranslateRequestHandler> _logger;

    public TranslateRequestHandler(
        ITranslatorService translatorService,
        ILogger<TranslateRequestHandler> logger)
    {
        _translatorService = translatorService;
        _logger = logger;
    }

    public async Task<TranslateResponse> Handle(TranslateMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var response = await _translatorService.Translate(request.Text, request.Source, request.Target, cancellationToken);

            return TranslateResponse.From(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}