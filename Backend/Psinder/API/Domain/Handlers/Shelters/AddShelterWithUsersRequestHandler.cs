using MediatR;
using Psinder.API.Auth.Common;
using Psinder.API.Common;
using Psinder.API.Common.Services.PasswordGeneratorService;
using Psinder.API.Domain.Models.Users;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Extensions;
using Psinder.DB.Common.Services;
using Psinder.DB.Domain.Repositories.Shelters;
using Psinder.DB.Domain.Repositories.Users;
using System.Text;
using System.Web;

namespace Psinder.API.Domain.Handlers;

public class AddShelterWithUsersRequestHandler : IRequestHandler<AddShelterWithUsersMediatr, AddShelterWithUsersResponse>
{
    private readonly IShelterRepository _shelterRepository;
    private readonly IPasswordValidationService _passwordValidationService;
    private readonly IEmailValidationService _emailValidationService;
    private readonly IHashService _hashService;
    private readonly ICacheService _cacheService;
    private readonly ITokenService _tokenService;
    private readonly ILoginRepository _loginRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ILogger<AddShelterWithUsersRequestHandler> _logger;

    public AddShelterWithUsersRequestHandler(
        IShelterRepository shelterRepository,
        IPasswordValidationService passwordValidationService,
        IEmailValidationService emailValidationService,
        IHashService hashService,
        ICacheService cacheService,
        ITokenService tokenService,
        ILoginRepository loginRepository,
        IUserRepository userRepository,
        IEmailSenderService emailSenderService,
        ILogger<AddShelterWithUsersRequestHandler> logger)
    {
        _shelterRepository = shelterRepository;
        _passwordValidationService = passwordValidationService;
        _emailValidationService = emailValidationService;
        _hashService = hashService;
        _cacheService = cacheService;
        _tokenService = tokenService;
        _loginRepository = loginRepository;
        _userRepository = userRepository;
        _emailSenderService = emailSenderService;
        _logger = logger;
    }

    public async Task<AddShelterWithUsersResponse> Handle(AddShelterWithUsersMediatr requestMediatr, CancellationToken cancellationToken)
    {
        try
        {
            var request = requestMediatr.Request;

            foreach (var worker in request.Workers!)
            {
                var validationResult = await ValidateRegistrationAttempt(worker, cancellationToken);

                if (validationResult.Status != AddShelterWithUsersResponseStatus.Ok)
                {
                    return validationResult;
                }
            }

            var domainModel = AddShelterWithUsersRequest.ToDomain(request);

            foreach(var worker in domainModel.Workers!)
            {
                var password = PasswordGenerator.GeneratePasswordString(16);

                _hashService.GeneratePasswordHash(Encoding.UTF8.GetBytes(password), out byte[] passwordSalt, out byte[] passwordHash);

                worker.User.LoginData.AccountStatusId = (byte)AccountStatuses.Unconfirmed;
                worker.User.LoginData.LoginAttempts = 0;
                worker.User.LoginData.Salt = passwordSalt;
                worker.User.LoginData.PasswordHash = passwordHash;
                worker.User.LoginData.PasswordLastChangeDate = DateTime.UtcNow;
                worker.User.LoginData.RegisterVerificationToken = await _tokenService.GenerateRegisterVerificationToken();

                var accessKeyconfigs = await _cacheService.GetConfigurationsByCategory(ConfigurationCategory.AccessKeyData, cancellationToken);
                var accessKeyLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessKeyLength);
                var accessSaltLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessSaltLength);
                var accessIVLength = accessKeyconfigs.GetConfigurationLongValue(ConfigurationCategory.AccessKeyData, ConfigurationItem.AccessIVLength);

                worker.User.LoginData.AccessKey = PasswordGenerator.GeneratePasswordBytes(accessKeyLength);
                worker.User.LoginData.AccessSalt = PasswordGenerator.GeneratePasswordBytes(accessSaltLength);
                worker.User.LoginData.AccessIV = PasswordGenerator.GeneratePasswordBytes(accessIVLength);

                var user = await _loginRepository.Register(worker.User, cancellationToken);
                worker.User.LoginData.Id = user.Id;
                worker.User.Id = user.LoginData.Id;

                var to = worker.User.LoginData.Email;
                var verificationToken = HttpUtility.UrlEncode(worker.User.LoginData.RegisterVerificationToken.Value);

                await _emailSenderService.SendVerificationWorkerEmail(to, verificationToken, password);
            }

            var shelterData = await _shelterRepository.AddShelter(domainModel, cancellationToken);

            return AddShelterWithUsersResponse.Create(AddShelterWithUsersResponseStatus.Ok, shelterData.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private async Task<AddShelterWithUsersResponse> ValidateRegistrationAttempt(AddShelterWithUsersRequest.WorkerInfo worker, CancellationToken cancellationToken)
    {
        if (await _loginRepository.CheckIfLoginIsAlreadyUsed(worker.Email, cancellationToken))
        {
            return new AddShelterWithUsersResponse(AddShelterWithUsersResponseStatus.LoginArleadyUsed, errorMessage: $"For email: {worker.Email}");
        }

        if (!_emailValidationService.ValidateEmail(worker.Email))
        {
            return new AddShelterWithUsersResponse(AddShelterWithUsersResponseStatus.InvalidEmail, errorMessage: $"For email: {worker.Email}");
        }

        if (await _userRepository.CheckIfEmailIsAlreadyUsed(worker.Email, cancellationToken))
        {
            return new AddShelterWithUsersResponse(AddShelterWithUsersResponseStatus.EmailArleadyUsed, errorMessage: $"For email: {worker.Email}");
        }

        return new AddShelterWithUsersResponse(AddShelterWithUsersResponseStatus.Ok);
    }
}