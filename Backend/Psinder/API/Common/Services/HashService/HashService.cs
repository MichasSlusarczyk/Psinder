using System.Security.Cryptography;

namespace Psinder.API.Common;

public class HashService : IHashService
{
    public void GeneratePasswordHash(byte[] password, out byte[] passwordSalt, out byte[] passwordHash)
    {
        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(password);
    }

    public bool VerifyPasswordHash(byte[] password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);

        var computedPasswordHash = hmac.ComputeHash(password);

        if (!computedPasswordHash.SequenceEqual(passwordHash))
        {
            return false;
        }

        return true;
    }

}
