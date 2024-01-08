namespace Psinder.DB.Auth.Models;

public class AccessKeyDataDto
{
    public long UserId { get; set; }
    public byte[] AccessKey { get; set; }
    public byte[] AccessSalt { get; set; }
    public byte[] AccessIV { get; set; }
}

