using Psinder.API.Auth.Models.Common;
using Psinder.DB.Auth.Entities;

namespace Psinder.API.Auth.Common
{
    public interface ITokenService
    {
        public Task<string> GenerateLoginToken(long userId, long loginId, string role);

        public Task<Token> GenerateRegisterVerificationToken();

        public Task<Token> GenerateResetPasswordToken();

        public Task<Token> GenerateRefreshToken();

        public DateTime GetTokenExpirationDate(string token);

        public DateTime GetTokenCreationDate(string token);

        public string? GetTokenClaimValue(string token, ClaimType claimType);
    }
}