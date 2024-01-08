using DB.Common.Services.Encription;
using FluentAssertions;

namespace Psinder.UnitTests.Tests.Auth.Passwords;

public class AESTests
{
    [Fact]
    public void CanCorrectlyEncryptWithAESTest()
    {
        // given
        var accessIV = "U/.<2?;]7DIFfr|?";
        var accessKey = "$Xjru?$>/nN^GC1PP?u^1";
        var accessSalt = "?j@H06<N<ol5l4ef";
        var textToEncrypt = "Password";

        // when
        var encryptedText = AES.Encrypt(textToEncrypt, accessKey, accessSalt, accessIV);
        var decryptedText = AES.Decrypt(encryptedText, accessKey, accessSalt, accessIV);

        // then
        textToEncrypt.Should().BeEquivalentTo(decryptedText);
    }
}
