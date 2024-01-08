using Psinder.DB.Auth.Entities;

namespace Psinder.DB.Auth.Models;

public class RemindPasswordDto
{
    public long? Id { get; set; }
    public string Login { get; set; }
    public byte AccountStatusId { get; set; }
    public long? ResetPasswordTokenId { get; set; }
    public Token? ResetPasswordToken { get; set; }
}
