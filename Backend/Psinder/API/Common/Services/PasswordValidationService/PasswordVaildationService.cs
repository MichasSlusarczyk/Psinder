using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Services;
using Psinder.DB.Common.Extensions;

namespace Psinder.API.Common;

public class PasswordVaildationService : IPasswordValidationService
{
    private readonly ICacheService _cacheService;

    public PasswordVaildationService(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    public class PasswordSyntaxValidationResult
    {
        public PasswordSyntaxValidationResult(bool result, string? errorMessage = null)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public bool Result { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public async Task<PasswordSyntaxValidationResult> ValidateLoginPassword(string password, CancellationToken cancellationToken)
    {
        var configurations = await _cacheService.GetConfigurationsByCategory(ConfigurationCategory.LoginPassword, cancellationToken);

        var loginPasswordMinLength = configurations.GetConfigurationLongValue(ConfigurationCategory.LoginPassword, ConfigurationItem.LoginPasswordMinLength);
        var lowercaseLettersMinCount = configurations.GetConfigurationLongValue(ConfigurationCategory.LoginPassword, ConfigurationItem.LowercaseLettersMinCount);
        var uppercaseLettersMinCount = configurations.GetConfigurationLongValue(ConfigurationCategory.LoginPassword, ConfigurationItem.UppercaseLettersMinCount);
        var digitsMinCount = configurations.GetConfigurationLongValue(ConfigurationCategory.LoginPassword, ConfigurationItem.DigitsMinCount);
        var specialCharactersMinCount = configurations.GetConfigurationLongValue(ConfigurationCategory.LoginPassword, ConfigurationItem.SpecialCharactersMinCount);

        if (password.Length < loginPasswordMinLength)
        {
            return new PasswordSyntaxValidationResult(false, "Password is to short.");
        }

        var lowercaseLettersCount = 0;
        var uppercaseLettersCount = 0;
        var digitsCount = 0;
        var specialCharactersCount = 0;

        foreach (char sign in password)
        {
            if (sign >= 'a' && sign <= 'z')
            {
                lowercaseLettersCount++;
            }
            else if (sign >= 'A' && sign <= 'Z')
            {
                uppercaseLettersCount++;
            }
            else if(sign >= '0' && sign <= '9')
            {
                digitsCount++;
            }
            else
            {
                specialCharactersCount++;
            }
        }

        if (lowercaseLettersCount < lowercaseLettersMinCount)
        {
            return new PasswordSyntaxValidationResult(false, "Too few lowercase letters.");
        }

        if (uppercaseLettersCount < uppercaseLettersMinCount)
        {
            return new PasswordSyntaxValidationResult(false, "Too few uppercase letters.");
        }

        if (digitsCount < digitsMinCount)
        {
            return new PasswordSyntaxValidationResult(false, "Too few digits.");
        }

        if (specialCharactersCount < specialCharactersMinCount)
        {
            return new PasswordSyntaxValidationResult(false, "Too few special characters.");
        }

        return new PasswordSyntaxValidationResult(true);
    }
}
