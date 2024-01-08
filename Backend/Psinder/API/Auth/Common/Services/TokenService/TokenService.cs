using Psinder.API.Auth.Models.Common;
using Microsoft.IdentityModel.Tokens;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Psinder.API.Auth.Common;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ICacheService _cacheService;

    public TokenService(IConfiguration configuration, ICacheService cacheService)
    {
        _configuration = configuration;
        _cacheService = cacheService;
    }

    public async Task<string> GenerateLoginToken(long userId, long loginId, string role)
    {
        var claims = new List<Claim>()
        {
            new Claim(Enum.GetName(ClaimType.UserId), userId.ToString()),
            new Claim(Enum.GetName(ClaimType.LoginId), loginId.ToString()),
            new Claim(Enum.GetName(ClaimType.Role), role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:TokenKey").Value));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var timestamp = DateTime.UtcNow;
        var time = await _cacheService.GetConfigurationIntValueByItem(
                ConfigurationCategory.Token,
                ConfigurationItem.LoginTokenExpireTimeInMinutes,
                new CancellationToken());

        var descriptor = new SecurityTokenDescriptor()
        {
            Expires = timestamp.AddDays(time),
            IssuedAt = timestamp,
            NotBefore = timestamp,
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims)
        };

        var hander = new JwtSecurityTokenHandler();
        var token = new JwtSecurityTokenHandler().CreateJwtSecurityToken(descriptor);

        return hander.WriteToken(token);
    }

    public async Task<Token> GenerateRegisterVerificationToken()
    {
        var timestamp = DateTime.UtcNow;
        var time = await _cacheService.GetConfigurationIntValueByItem(
                ConfigurationCategory.Token,
                ConfigurationItem.VerificationTokenExpireTimeInMinutes,
                new CancellationToken());

        return new Token()
        {
            Value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)),
            CreationDate = DateTime.UtcNow,
            ExpirationDate = timestamp.AddMinutes(time)
        };
    }

    public async Task<Token> GenerateResetPasswordToken()
    {
        var timestamp = DateTime.UtcNow;
        var time = await _cacheService.GetConfigurationIntValueByItem(
                ConfigurationCategory.Token,
                ConfigurationItem.ResetPasswordTokenExpireTimeInMinutes,
                new CancellationToken());

        return new Token()
        {
            Value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16)),
            CreationDate = DateTime.UtcNow,
            ExpirationDate = timestamp.AddMinutes(time)
        };
    }

    public async Task<Token> GenerateRefreshToken()
    {
        var timestamp = DateTime.UtcNow;
        var time = await _cacheService.GetConfigurationIntValueByItem(
                ConfigurationCategory.Token,
                ConfigurationItem.RefreshTokenExpireTimeInMinutes,
                new CancellationToken());

        return new Token()
        {
            Value = Convert.ToBase64String(RandomNumberGenerator.GetBytes(128)),
            CreationDate = DateTime.UtcNow,
            ExpirationDate = timestamp.AddDays(time)
        };
    }

    public DateTime GetTokenExpirationDate(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

        return jwt.ValidTo;
    }

    public DateTime GetTokenCreationDate(string token)
    {
        var jwt = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

        return jwt.ValidFrom;
    }

    public string? GetTokenClaimValue(string token, ClaimType claimType)
    {
        var jwt = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

        return jwt.Claims.SingleOrDefault(x => x.Type == Enum.GetName(claimType))?.Value;
    }
}