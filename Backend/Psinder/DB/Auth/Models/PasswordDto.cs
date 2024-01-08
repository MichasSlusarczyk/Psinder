using Psinder.DB.Auth.Entities;

namespace Psinder.DB.Auth.Models;

public class PasswordDto
{
    public long Id { get; set; }
    public byte[]? Salt { get; set; }
    public byte[]? PasswordHash { get; set; }
    public AccountStatuses AccountStatus { get; set; }
}

