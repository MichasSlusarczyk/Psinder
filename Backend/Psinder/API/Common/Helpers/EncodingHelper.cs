using Psinder.API.Common.Models;
using System.Text;

namespace Psinder.API.Common.Helpers;

public static class EncodingHelper
{
    public  static Encoding GetEncoding(Encodings encoding)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        switch(encoding)
        {
            case Encodings.UTF8:
                return Encoding.UTF8;
            case Encodings.W1250:
                return Encoding.GetEncoding("windows-1250");
            default:
                throw new NotSupportedException("Encoding type not supported.");
        }
    }

    public static byte[] GetBytes(string text, Encodings encoding)
    {
        var enc = GetEncoding(encoding);
        return enc.GetBytes(text);
    }
}
