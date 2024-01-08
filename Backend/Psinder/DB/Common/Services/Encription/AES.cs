using System.Security.Cryptography;
using System.Text;

namespace DB.Common.Services.Encription;

public static class AES
{
    private static readonly int _iterations = 2;
    private static readonly int _keySize = 256;
    private static readonly string _hash = "SHA1";

    public static string Encrypt(string value, string password, string salt, string IV)
    {
        byte[] vectorBytes = Encoding.ASCII.GetBytes(IV);
        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
        byte[] valueBytes = Encoding.ASCII.GetBytes(value);

        byte[] encrypted;

        using (var cipher = Aes.Create())
        {
            var passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
            var keyBytes = passwordBytes.GetBytes(_keySize / 8);

            cipher.Mode = CipherMode.CBC;

            using (var encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        writer.Write(valueBytes, 0, valueBytes.Length);
                        writer.FlushFinalBlock();
                        encrypted = memoryStream.ToArray();
                    }
                }
            }

            cipher.Clear();
        }

        return Convert.ToBase64String(encrypted);
    }

    public static string Decrypt(string value, string password, string salt, string IV)
    {


        byte[] vectorBytes = Encoding.ASCII.GetBytes(IV);
        byte[] saltBytes = Encoding.ASCII.GetBytes(salt);
        byte[] valueBytes = Convert.FromBase64String(value);

        string decrypted;

        using (var cipher = Aes.Create())
        {

            var passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
            var keyBytes = passwordBytes.GetBytes(_keySize / 8);

            cipher.Mode = CipherMode.CBC;

            try
            {
                using (var dencryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
                {
                    using (var memoryStream = new MemoryStream(valueBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, dencryptor, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(cryptoStream))
                            {
                                decrypted = reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            cipher.Clear();
        }

        return decrypted;
    }
}
