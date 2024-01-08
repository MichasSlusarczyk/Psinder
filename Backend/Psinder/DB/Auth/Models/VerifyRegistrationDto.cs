using Psinder.DB.Auth.Entities;

namespace Psinder.DB.Auth.Models;

public class VerifyRegistrationDto
{
    public long? Id { get; set; }
    public string Login { get; set; }
    public byte AccountStatusId { get; set; }
    public long? RegisterVerificationTokenId { get; set; }
    public Token? RegisterVerificationToken { get; set; }
}
