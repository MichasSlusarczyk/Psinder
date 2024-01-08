using Psinder.DB.Auth.Entities;

namespace Psinder.DB.Auth.Models;

public class RefreshTokenDto
{
    public long LoginId { get; set; }
    public long UserId { get; set; }
    public byte RoleId { get; set; }
    public byte AccountStatusId { get; set; }
    public long? RefreshTokenId { get; set; }
    public virtual Token? RefreshToken { get; set; }
}
