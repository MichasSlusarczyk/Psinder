using static Psinder.API.Common.PasswordVaildationService;

namespace Psinder.API.Common;

public interface IPasswordValidationService
{
    public Task<PasswordSyntaxValidationResult> ValidateLoginPassword(string password, CancellationToken cancellationToken);
}
