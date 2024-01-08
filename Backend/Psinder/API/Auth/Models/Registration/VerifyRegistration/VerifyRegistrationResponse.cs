namespace Psinder.API.Auth.Models.Registrations;

public class VerifyRegistrationResponse
{
    public VerifyRegistrationResponse(VerifyRegistrationStatus verifyRegistrationStatus)
    {
        Status = verifyRegistrationStatus;
    }

    public VerifyRegistrationStatus Status { get; set; }
}
