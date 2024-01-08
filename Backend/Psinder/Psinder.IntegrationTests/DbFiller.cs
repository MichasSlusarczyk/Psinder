using Psinder.DB.Auth.Entities;
using Psinder.DB.Common.Extensions;
using Psinder.DB.Domain.Entities;
using System.Text;

namespace Psinder.IntegrationTests;

public class DbFiller
{
    readonly private Fixtures _fixtures;

    public DbFiller(Fixtures fixtures)
    {
        _fixtures = fixtures;
    }

    public async Task AddCommonUserWithLoginData()
    {
        var userDetails = ReturnCommonUserDetails();
        var user = ReturnCommonUser();
        var loginData = ReturnCommonLoginData();

        await _fixtures.Add(userDetails);
        await _fixtures.Add(user);
        await _fixtures.Add(loginData);
    }

    private User ReturnCommonUser()
    {
        return new User()
        {
            Id = 1,
            UserDetailsId = 1,
            CreationDate = DateTime.Now
        };
    }

    private UserDetails ReturnCommonUserDetails()
    {
        return new UserDetails()
        {
            Id = 1,
            FirstName = "FirstName",
            LastName = "LastName",
            PhoneNumber = "123456789"
        };
    }

    private LoginData ReturnCommonLoginData()
    {
        return new LoginData()
        {
            Id = 1,
            UserId= 1,
            LoginAttempts = 0,
            Email = "email@gmail.com",
            RoleId = (byte)Role.User,
            Salt = ByteHelper.HexStringToByteArray("08B813DFE48D5FD411CDCC1A775018320343866D1DEB678A9AD0970D16F57EBFF8FE988B084D9C29EE5DC997055E90D82EA2CC97D2296F82168CA2B16445F20FB63818D14C31CAA29E53A0473E11656EBCCF322C83D9393972A6262C5D42580CA25DE3DEEBB34857B39FFBC1BE9352C2830150953781E42FC2259AAD2AB7AF27"),
            PasswordHash = ByteHelper.HexStringToByteArray("AD91DA8E831654B2A2B94AAB0E544CFEE32509DF0E6C4FD389C961E964B066A314DB939397846CCEA4CED302BD6BFEEBA2772934A8DE2C071A4A25D04C423F3B"),
            AccessIV = Encoding.ASCII.GetBytes("U/.<2?;]7DIFfr|?"),
            AccessKey = Encoding.ASCII.GetBytes("$Xjru?$>/nN^GC1PP?u^1"),
            AccessSalt = Encoding.ASCII.GetBytes("?j@H06<N<ol5l4ef"),
            AccountStatusId = (byte)AccountStatuses.Active,
            ResetPasswordTokenId = null,
            RegisterVerificationTokenId = null,
            RefreshTokenId = null,
            PasswordLastChangeDate = null
        };
    }
}
