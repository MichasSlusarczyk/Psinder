using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Repositories.Files;
using Psinder.DB.Common.Repositories.UnitOfWorks;

namespace Psinder.DB.Domain.Repositories.Appointments;

public class FileRepository : IFileRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public FileRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Common.Entities.File?> GetFileById(long fileId, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.FilesEntity.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
    }

    public async Task DeleteFile(Common.Entities.File file, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.FilesEntity.Remove(file);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task DeleteFileById(long fileId, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.DatabaseContext.FilesEntity.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);

        if(file != null)
        {
            _unitOfWork.DatabaseContext.FilesEntity.Remove(file);
            await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
            _unitOfWork.DatabaseContext.ChangeTracker.Clear();
        }
    }

    public async Task<long> AddFileData(Common.Entities.File file, byte[] content, CancellationToken cancellationToken)
    {
        file.Content = content;

        _unitOfWork.DatabaseContext.FilesEntity.Update(file);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        return file.Id;
    }

    public async Task<long> AddFile(Common.Entities.File file, CancellationToken cancellationToken)
    {
        _unitOfWork.DatabaseContext.FilesEntity.Add(file);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        return file.Id;
    }

    public async Task<byte[]?> GetFileData(long fileId, CancellationToken cancellationToken)
    {
        var file = await _unitOfWork.DatabaseContext.FilesEntity.FirstOrDefaultAsync(x => x.Id == fileId, cancellationToken);
        
        return file?.Content;
    }
}