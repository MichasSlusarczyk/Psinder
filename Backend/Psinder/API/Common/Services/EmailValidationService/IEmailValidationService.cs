namespace Psinder.API.Common;

public interface IEmailValidationService
{
    public bool ValidateEmail(string email);
}
