namespace Psinder.DB.Common.Repositories.Files;

public interface IFileRepository
{
    Task<Common.Entities.File?> GetFileById(long fileId, CancellationToken cancellationToken);

    Task DeleteFile(Common.Entities.File file, CancellationToken cancellationToken);

    Task DeleteFileById(long fileId, CancellationToken cancellationToken);

    Task<long> AddFileData(Common.Entities.File file, byte[] content, CancellationToken cancellationToken);

    Task<long> AddFile(Common.Entities.File file, CancellationToken cancellationToken);

    Task<byte[]?> GetFileData(long fileId, CancellationToken cancellationToken);
}
