using Psinder.DB.Auth.Entities;

namespace Psinder.API.Common;

public interface IIdentityService
{
    public long? CurrentUserId { get; }
    public long? CurrentLoginId { get; }
    public Role? CurrentUserRole { get; }

    public bool VerifyAccess(long userId);

    public bool VerifyAccessForAdmin(long userId);

    public bool VerifyAccessForWorker(long userId);

    public bool VerifyAccessForWorkerAndAdmin(long userId);

    public bool VerifyAccessForAdmin(List<long> usersIds);
}
