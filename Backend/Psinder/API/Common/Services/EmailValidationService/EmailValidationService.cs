using System.ComponentModel.DataAnnotations;

namespace Psinder.API.Common;

public class EmailValidationService : IEmailValidationService
{
    public bool ValidateEmail(string email)
    {
        if(email == null)
        {
            return false;
        }
        
        if (!new EmailAddressAttribute().IsValid(email))
        {
            return false;
        }

        return true;
    }
}
