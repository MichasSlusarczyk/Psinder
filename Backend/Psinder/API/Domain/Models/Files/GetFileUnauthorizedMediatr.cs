using MediatR;

namespace Psinder.API.Domain.Models.Files;

public class GetFileUnauthorizedMediatr : IRequest<FileResponse>
{
    public GetFileUnauthorizedMediatr(GetFileRequest request)
    {
        Request = request;
    }

    public GetFileRequest Request { get; set; }
}
