using MediatR;
using Psinder.API.Domain.Models;
using Psinder.API.Domain.Models.Files;
using Psinder.DB.Common.Repositories.Files;

namespace Psinder.API.Domain.Handlers.Files;

public class GetFileUnauthorizedRequestHandler : IRequestHandler<GetFileUnauthorizedMediatr, FileResponse>
{
    private readonly IFileRepository _fileRepository;
    private readonly ILogger<GetFileUnauthorizedRequestHandler> _logger;

    public GetFileUnauthorizedRequestHandler(
        IFileRepository fileRepository,
        ILogger<GetFileUnauthorizedRequestHandler> logger)
    {
        _fileRepository = fileRepository;
        _logger = logger;
    }

    public async Task<FileResponse> Handle(GetFileUnauthorizedMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            var file = await _fileRepository.GetFileById(request.FileId, cancellationToken)
                ?? throw new Exception($"File with ID: {request.FileId} not found.");

            return FileResponse.FromDomain(file);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}