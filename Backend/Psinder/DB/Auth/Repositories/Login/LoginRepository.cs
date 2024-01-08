using Microsoft.EntityFrameworkCore;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Domain.Entities;

namespace Psinder.DB.Auth.Repositories.Logins;

public class LoginRepository : ILoginRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public LoginRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginValidationDto?> GetLoginValidationData(string login, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.Email == login)
            .Select(x => new LoginValidationDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                Login = x.Email,
                Salt = x.Salt,
                PasswordHash = x.PasswordHash,
                LoginAttempts = x.LoginAttempts,
                AccountStatusId = x.AccountStatusId,
                RoleId = x.RoleId
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task Login(long loginId, Token refreshToken, CancellationToken cancellationToken)
    {
        refreshToken.Id = _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.Id == loginId)
            .Select(x => x.RefreshTokenId)
            .SingleOrDefault() ?? 0;

        _unitOfWork.DatabaseContext.TokensEntity.Update(refreshToken);

        var logindata = new LoginData()
        {
            Id = loginId,
            LoginAttempts = 0,
            RefreshToken = refreshToken
        };

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(logindata);
        entry.Property(x => x.LoginAttempts).IsModified = true;
        entry.Property(x => x.RefreshTokenId).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task UpdateLoginAttemptsCount(long loginId, byte loginAttempts, CancellationToken cancellationToken)
    {
        var loginData = new LoginData()
        {
            Id = loginId,
            LoginAttempts = loginAttempts,
        };

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(loginData);
        entry.Property(x => x.LoginAttempts).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<RefreshTokenDto?> GetRefreshTokenData(string refreshToken, CancellationToken cancellationToken)
    {
        var tokenData = await _unitOfWork.DatabaseContext.TokensEntity.SingleOrDefaultAsync(x => x.Value == refreshToken);

        if(tokenData == null)
        {
            return null;
        }

        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Include(x => x.RefreshToken)
            .Where(x => x.Id == tokenData.Id)
            .Select(x => new RefreshTokenDto()
            {
                LoginId = x.Id,
                UserId = x.UserId,
                AccountStatusId = x.AccountStatusId,
                RoleId = x.RoleId,
                RefreshToken = tokenData,
                RefreshTokenId = tokenData.Id

            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task RefreshToken(long loginId, Token refreshToken, CancellationToken cancellationToken)
    {
        refreshToken.Id = _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.Id == loginId)
            .Select(x => x.RefreshTokenId)
            .SingleOrDefault() ?? 0;

        _unitOfWork.DatabaseContext.TokensEntity.Update(refreshToken);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task UpdateResetPasswordToken(long loginId, Token resetPasswordToken, CancellationToken cancellationToken)
    {
        var loginData = _unitOfWork.DatabaseContext.LoginsDataEntity
            .Include(x => x.ResetPasswordToken)
            .Where(x => x.Id == loginId)
            .SingleOrDefault();

        if(loginData!.ResetPasswordTokenId != null)
        {
            resetPasswordToken.Id = loginData.ResetPasswordTokenId.Value;
        }

        loginData.ResetPasswordToken = resetPasswordToken;

        var token = _unitOfWork.DatabaseContext.LoginsDataEntity.Update(loginData);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<PasswordDto?> GetPasswordData(long userId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.UserId == userId)
            .Select(x => new PasswordDto()
            {
                Id = x.Id,
                PasswordHash = x.PasswordHash,
                Salt = x.Salt,
                AccountStatus = (AccountStatuses)x.AccountStatusId
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<PasswordDto?> GetResetPasswordData(string remindPasswordToken, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.ResetPasswordToken!.Value == remindPasswordToken)
            .Select(x => new PasswordDto()
            {
                Id= x.Id,
                PasswordHash = x.PasswordHash,
                Salt = x.Salt,
                AccountStatus = (AccountStatuses)x.AccountStatusId
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<AccessKeyDataDto?> GetAccessKeyData(long userId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.UserId == userId)
            .Select(x => new AccessKeyDataDto()
            {
                UserId = x.UserId,
                AccessKey = x.AccessKey,
                AccessSalt = x.AccessSalt,
                AccessIV = x.AccessIV  
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task ChangePassword(long loginId, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken)
    {
        var loginData = new LoginData()
        {
            Id = loginId,
            PasswordHash = passwordHash,
            Salt = passwordSalt
        };

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(loginData);
        entry.Property(x => x.PasswordHash).IsModified = true;
        entry.Property(x => x.Salt).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task ResetPassword(long loginId, byte[] passwordHash, byte[] passwordSalt, CancellationToken cancellationToken)
    {
        var loginData = new LoginData()
        {
            Id = loginId,
            PasswordHash = passwordHash,
            Salt = passwordSalt,
            PasswordLastChangeDate= DateTime.UtcNow,
            ResetPasswordToken = null
        };

        var token = _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.Id == loginId)
            .Select(x => x.ResetPasswordToken)
            .SingleOrDefault();

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(loginData);
        entry.Property(x => x.PasswordHash).IsModified = true;
        entry.Property(x => x.Salt).IsModified = true;
        entry.Property(x => x.PasswordLastChangeDate).IsModified = true;
        entry.Property(x => x.ResetPasswordTokenId).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        _unitOfWork.DatabaseContext.TokensEntity.Attach(token);
        _unitOfWork.DatabaseContext.TokensEntity.Remove(token);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task UpdateAccountStatus(long loginId, AccountStatuses accountStatus, CancellationToken cancellationToken)
    {
        var loginData = new LoginData()
        {
            Id = loginId,
            AccountStatusId = (byte)accountStatus,
        };

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(loginData);
        entry.Property(x => x.AccountStatusId).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<User> Register(User user, CancellationToken cancellationToken)
    {
        await _unitOfWork.DatabaseContext.AddAsync(user);
        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        return user;
    }

    public async Task<VerifyRegistrationDto?> GetVerifyRegistrationData(string registerVerificationToken, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.RegisterVerificationToken!.Value == registerVerificationToken)
            .Select(x => new VerifyRegistrationDto()
            {
                Id = x.Id,
                Login = x.Email,
                AccountStatusId = x.AccountStatusId,
                RegisterVerificationTokenId = x.RegisterVerificationTokenId,
                RegisterVerificationToken = x.RegisterVerificationToken
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<RemindPasswordDto?> GetRemindPasswordData(string remindPasswordToken, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.ResetPasswordToken!.Value == remindPasswordToken)
            .Select(x => new RemindPasswordDto()
            {
                Id = x.Id,
                Login = x.Email,
                AccountStatusId = x.AccountStatusId,
                ResetPasswordTokenId = x.ResetPasswordTokenId,
                ResetPasswordToken = x.ResetPasswordToken
            }).SingleOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task VerifyRegistration(long loginId, CancellationToken cancellationToken)
    {
        var loginData = new LoginData()
        {
            Id = loginId,
            AccountStatusId = (byte)AccountStatuses.Active,
            RegisterVerificationToken = null
        };

        var token = _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.Id == loginId)
            .Select(x => x.RegisterVerificationToken)
            .SingleOrDefault();

        var entry = _unitOfWork.DatabaseContext.LoginsDataEntity.Attach(loginData);
        entry.Property(x => x.AccountStatusId).IsModified = true;
        entry.Property(x => x.RegisterVerificationTokenId).IsModified = true;

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();

        _unitOfWork.DatabaseContext.TokensEntity.Attach(token);
        _unitOfWork.DatabaseContext.TokensEntity.Remove(token);

        await _unitOfWork.DatabaseContext.SaveChangesAsync(cancellationToken);
        _unitOfWork.DatabaseContext.ChangeTracker.Clear();
    }

    public async Task<bool> CheckIfLoginIsAlreadyUsed(string login, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .AnyAsync(x => x.Email == login, cancellationToken);

        return result;
    }

    public async Task<long?> GetLoginIdByUserId(long userId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                Id = x.Id,
            }).SingleOrDefaultAsync(cancellationToken);

        return result?.Id;
    }

    public async Task<string?> GetLoginByUserId(long userId, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.DatabaseContext.LoginsDataEntity
            .Where(x => x.UserId == userId)
            .Select(x => new
            {
                Login = x.Email,
            }).SingleOrDefaultAsync(cancellationToken);

        return result?.Login;
    }
}
