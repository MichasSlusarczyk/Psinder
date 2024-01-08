using DB.Common.Services.Encription;
using System.Text;
using System.Text.Json;

namespace Psinder.DB.Common.Extensions;

public static class CacheSerializationExtensions
{
    public static byte[] ToByteArray(this object obj, string password, string salt, string IV)
    {
        var jsonSerialize = JsonSerializer.Serialize(obj);
        var jsonEncrypted = AES.Encrypt(jsonSerialize, password, salt, IV);
        return Encoding.ASCII.GetBytes(jsonEncrypted);
    }

    public static T FromByteArray<T>(this byte[] byteArray, string password, string salt, string IV)
    {
        if (byteArray == null)
        {
            return default(T);
        }

        var jsonEncrypted = Encoding.ASCII.GetString(byteArray);
        var jsonSerialize = AES.Decrypt(jsonEncrypted, password, salt, IV);
        return JsonSerializer.Deserialize<T>(jsonSerialize);
    }
}
