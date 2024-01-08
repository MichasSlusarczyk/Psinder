using System.Text;

namespace Psinder.API.Common.Services.PasswordGeneratorService;

public class PasswordGenerator
{
    public static string GeneratePasswordString(
        long length, 
        bool useSmallLetters = true,
        bool useBigLetters = true,
        bool useNumbers = true,
        bool useSpecialCharacters = true)
    {
        var sb = new StringBuilder();
        var random = new Random();
        const string smallLetters = @"abcdefghijklmnopqrstuvwxyz";
        const string bigLetters = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = @"0123456789";
        const string specialCharacters = @"!”#$%&’()*+,-./:;<=>?@[\]^_`{|}~";

        if (useSmallLetters)
        {
            sb.Append(smallLetters);
        }

        if (useBigLetters)
        {
            sb.Append(bigLetters);
        }

        if (useNumbers)
        {
            sb.Append(numbers);
        }

        if (useSpecialCharacters)
        {
            sb.Append(specialCharacters);
        }

        string chars = sb.ToString();

        var str = new char[length];

        for (int i = 0; i < str.Length; i++)
        {
            str[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(str);

        return finalString;
    }

    public static byte[] GeneratePasswordBytes(
    long length,
    bool useSmallLetters = true,
    bool useBigLetters = true,
    bool useNumbers = true,
    bool useSpecialCharacters = true)
    {
        var finalString = GeneratePasswordString(length, useSmallLetters, useBigLetters, useNumbers, useSpecialCharacters);

        return Encoding.ASCII.GetBytes(finalString);
    }
}
