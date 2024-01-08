using Psinder.API.Common.Extensions;
using Psinder.DB.Auth.Entities;

namespace Psinder.API.Common;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContentAccessor;

    public IdentityService(IHttpContextAccessor httpContentAccessor)
    {
        _httpContentAccessor = httpContentAccessor;
    }

    public long? CurrentUserId => _httpContentAccessor.HttpContext!.GetLoggedUserId();
    public long? CurrentLoginId => _httpContentAccessor.HttpContext!.GetLoggedLoginId();
    public Role? CurrentUserRole => _httpContentAccessor.HttpContext!.GetLoggedUserRole();

    public bool VerifyAccess(long userId)
    {
        if(CurrentUserId.HasValue && userId == CurrentUserId)
        {
            return true;
        }

        return false;
    }

    public bool VerifyAccessForAdmin(long userId)
    {
        if (CurrentUserRole.HasValue && CurrentUserRole == Role.Admin)
        {
            return true;
        }
        else if (CurrentUserId.HasValue && userId == CurrentUserId)
        {
            return true;
        }

        return false;
    }

    public bool VerifyAccessForWorkerAndAdmin(long userId)
    {
        if (CurrentUserRole.HasValue && (CurrentUserRole == Role.Admin || CurrentUserRole == Role.Worker))
        {
            return true;
        }
        else if (CurrentUserId.HasValue && userId == CurrentUserId)
        {
            return true;
        }

        return false;
    }

    public bool VerifyAccessForWorker(long userId)
    {
        if (CurrentUserRole.HasValue && (CurrentUserRole == Role.Worker))
        {
            return true;
        }
        else if (CurrentUserId.HasValue && userId == CurrentUserId)
        {
            return true;
        }

        return false;
    }

    public bool VerifyAccessForAdmin(List<long> usersIds)
    {
        if (CurrentUserRole.HasValue && CurrentUserRole == Role.Admin)
        {
            return true;
        }
        else if (CurrentUserId.HasValue && usersIds.Any(userId => userId == CurrentUserId))
        {
            return true;
        }

        return false;
    }
}
