namespace Psinder.API.Common;

public interface IHashService
{
    public void GeneratePasswordHash(byte[] password, out byte[] passwordSalt, out byte[] passwordHash);

    public bool VerifyPasswordHash(byte[] password, byte[] passwordHash, byte[] passwordSalt);
}
