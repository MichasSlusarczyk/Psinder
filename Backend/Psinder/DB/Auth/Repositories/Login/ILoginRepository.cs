using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Auth.Repositories.Logins;

public interface ILoginRepository
{
    public Task<LoginValidationDto?> GetLoginValidationData(string login, CancellationToken cancellationToken);

    public Task Login(long loginId, Token refreshToken, CancellationToken cancellationToken);

    public Task UpdateLoginAttemptsCount(long loginId, byte loginAttempts, CancellationToken cancellationToken);

    public Task<RefreshTokenDto?> GetRefreshTokenData(string refreshToken, CancellationToken cancellationToken);

    public Task RefreshToken(long loginId, Token refreshToken, CancellationToken cancellationToken);

    public Task UpdateResetPasswordToken(long loginId, Token resetPasswordToken, CancellationToken cancellationToken);

    public Task<RemindPasswordDto?> GetRemindPasswordData(string remindPasswordToken, CancellationToken cancellationToken);

    public Task<PasswordDto?> GetPasswordData(long userId, CancellationToken cancellationToken);

    public Task<PasswordDto?> GetResetPasswordData(string remindPasswordToken, CancellationToken cancellationToken);

    public Task<AccessKeyDataDto?> GetAccessKeyData(long userId, CancellationToken cancellationToken);

    public Task ChangePassword(long loginId, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken);

    public Task ResetPassword(long loginId, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken);

    public Task UpdateAccountStatus(long loginId, AccountStatuses accountStatus, CancellationToken cancellationToken);

    public Task<User> Register(User user, CancellationToken cancellationToken);

    public Task<VerifyRegistrationDto?> GetVerifyRegistrationData(string registerVerificationToken, CancellationToken cancellationToken);

    public Task VerifyRegistration(long loginId, CancellationToken cancellationToken);

    public Task<bool> CheckIfLoginIsAlreadyUsed(string login, CancellationToken cancellationToken);

    public Task<long?> GetLoginIdByUserId(long userId, CancellationToken cancellationToken);

    public Task<string?> GetLoginByUserId(long userId, CancellationToken cancellationToken);
}
