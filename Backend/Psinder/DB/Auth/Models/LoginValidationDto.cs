namespace Psinder.DB.Auth.Models;

public class LoginValidationDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Login { get; set; }
    public byte[] Salt { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte LoginAttempts { get; set; }
    public byte RoleId { get; set; }
    public byte AccountStatusId { get; set; }
}
