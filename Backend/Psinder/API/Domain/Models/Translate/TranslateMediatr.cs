using MediatR;

namespace Psinder.API.Domain.Models.Translate;

public class TranslateMediatr : IRequest<TranslateResponse>
{
    public TranslateRequest Request { get; set; }
}