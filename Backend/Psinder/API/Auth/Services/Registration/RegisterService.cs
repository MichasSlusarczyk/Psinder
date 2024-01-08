using Psinder.API.Auth.Common;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Registrations;
using Psinder.API.Auth.Services.Login;
using Psinder.API.Common;
using Psinder.API.Common.Services.PasswordGeneratorService;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Extensions;
using Psinder.DB.Common.Services;
using Psinder.DB.Domain.Entities;
using Psinder.DB.Domain.Repositories.Users;
using System.Text;
using System.Web;

namespace Psinder.API.Auth.Services.Registration;

public class RegisterService : IRegisterService
{
    private readonly IPasswordValidationService _passwordValidationService;
    private readonly IHashService _hashService;
    private readonly ICacheService _cacheService;
    private readonly ITokenService _tokenService;
    private readonly ILoginRepository _loginRepostory;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ILoginRepository _loginRepository;
    private readonly IEmailValidationService _emailValidationService;
    private readonly IUserRepository _userRepostory;
    private readonly ILoginService _loginService;

    public RegisterService(
        IPasswordValidationService passwordValidationService,
        IHashService hashService,
        ICacheService cacheService,
        ITokenService tokenService,
        ILoginRepository loginRepostory,
        IEmailSenderService emailSenderService,
        ILoginRepository loginRepository,
        IEmailValidationService emailValidationService,
        IUserRepository userRepostory,
        ILoginService LoginService)
    {
        _passwordValidationService = passwordValidationService;
        _hashService = hashService;
        _cacheService = cacheService;
        _tokenService = tokenService;
        _loginRepostory = loginRepostory;
        _emailSenderService = emailSenderService;
        _loginRepository = loginRepository;
        _emailValidationService = emailValidationService;
        _userRepostory = userRepostory;
        _loginService = LoginService;
    }

    public async Task<AuthResponse> Register(RegistrationRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRegistrationAttempt(request.Email, cancellationToken);
        if (validationResult.Status != AuthStatus.Ok)
        {
            return validationResult;
        }

        var passwordSyntaxValidationResult = await _passwordValidationService.ValidateLoginPassword(request.Password, cancellationToken);
        if (!passwordSyntaxValidationResult.Result)
        {
            return new AuthResponse(AuthStatus.RegistrationInvalidPassword, errorMessage: passwordSyntaxValidationResult.ErrorMessage);
        }

        var domainModel = request.ToDomain();

        _hashService.GeneratePasswordHash(Encoding.UTF8.GetBytes(request.Password), out byte[] passwordSalt, out byte[] passwordHash);

        domainModel.LoginData.Salt = passwordSalt;
        domainModel.LoginData.PasswordHash = passwordHash;
        domainModel.LoginData.AccountStatusId = (byte)AccountStatuses.Unconfirmed;
        domainModel.LoginData.LoginAttempts = 0;
        domainModel.LoginData.PasswordLastChangeDate = DateTime.UtcNow;
        domainModel.LoginData.RegisterVerificationToken = await _tokenService.GenerateRegisterVerificationToken();

        var accessKeyconfigs = await _cacheService.GetConfigurationsByCategory(ConfigurationCategory.AccessKeyData, cancellationToken);
        var accessKeyLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessKeyLength);
        var accessSaltLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessSaltLength);
        var accessIVLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessIVLength);

        domainModel.LoginData.AccessKey = PasswordGenerator.GeneratePasswordBytes(accessKeyLength);
        domainModel.LoginData.AccessSalt = PasswordGenerator.GeneratePasswordBytes(accessSaltLength);
        domainModel.LoginData.AccessIV = PasswordGenerator.GeneratePasswordBytes(accessIVLength);

        var user = await _loginRepostory.Register(domainModel, cancellationToken);
        if(request.ShelterId != null)
        {
            await _userRepostory.AddWorker(new Worker() { UserId = user.Id, ShelterId = request.ShelterId.Value }, cancellationToken);
        }

        var to = domainModel.LoginData.Email;
        var verificationToken = HttpUtility.UrlEncode(domainModel.LoginData.RegisterVerificationToken.Value);

        await _emailSenderService.SendVerificationEmail(to, verificationToken);

        return new AuthResponse(AuthStatus.Ok);
    }

    public async Task<AuthResponse> RegisterWithService(string email, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRegistrationAttempt(email, cancellationToken);

        if (validationResult.Status != AuthStatus.Ok)
        {
            return validationResult;
        }

        var domainModel = User.ToDomain(email);

        domainModel.LoginData.AccountStatusId = (byte)AccountStatuses.Active;
        domainModel.LoginData.LoginAttempts = 0;
        domainModel.LoginData.PasswordLastChangeDate = DateTime.UtcNow;

        await _loginRepository.Register(domainModel, cancellationToken);

        return await _loginService.Login(email, cancellationToken);
    }

    private async Task<AuthResponse> ValidateRegistrationAttempt(string email, CancellationToken cancellationToken)
    {
        if (await _loginRepository.CheckIfLoginIsAlreadyUsed(email, cancellationToken))
        {
            return new AuthResponse(AuthStatus.LoginArleadyUsed);
        }

        if (!_emailValidationService.ValidateEmail(email))
        {
            return new AuthResponse(AuthStatus.InvalidEmail);
        }

        if (await _userRepostory.CheckIfEmailIsAlreadyUsed(email, cancellationToken))
        {
            return new AuthResponse(AuthStatus.EmailArleadyUsed);
        }

        return new AuthResponse(AuthStatus.Ok);
    }
}
