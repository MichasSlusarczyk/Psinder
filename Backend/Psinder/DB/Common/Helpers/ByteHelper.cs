namespace Psinder.DB.Common.Extensions;

public static class ByteHelper
{
    public static byte[] HexStringToByteArray(string hex)
    {
        return Enumerable.Range(0, hex.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                         .ToArray();
    }

    public static byte[] Base64ToByteArray(string base64)
    {
        return Convert.FromBase64String(base64);
    }
}
