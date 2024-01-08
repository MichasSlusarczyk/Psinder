using Psinder.API.Auth.Common;
using Psinder.API.Auth.Handlers;
using Psinder.API.Auth.Models.Logins;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Psinder.DB.Auth.Entities;
using Psinder.DB.Auth.Models;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.API.Common;

namespace Psinder.UnitTests.Tests.Auth.Registration;

public class RefreshTokenHandlerTests
{

    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<ILoginRepository> _mockLoginRepository;
    private readonly Mock<IIdentityService> _mockIdentityService;
    private readonly Mock<ILogger<RefreshTokenRequestHandler>> _mockLogger;

    public RefreshTokenHandlerTests()
    {
        _mockTokenService = new Mock<ITokenService>();
        _mockLoginRepository = new Mock<ILoginRepository>();
        _mockIdentityService = new Mock<IIdentityService>();
        _mockLogger = new Mock<ILogger<RefreshTokenRequestHandler>>();
    }

    [Fact]
    public async Task CanRefreshTokenTest()
    {
        // given
        var request = new RefreshTokenRequest()
        {
            RefreshToken = "RefreshToken"
        };
        var refreshTokenData = new RefreshTokenDto()
        {
            LoginId = 1,
            UserId = 1,
            RoleId = 2,
            AccountStatusId = 2,
            RefreshTokenId = 1,
            RefreshToken = new Token()
            {
                Id = 1,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1),
                Value = "RefreshToken"
            }
        };
        var newLoginToken = "newLoginToken";
        var newRefreshToken = new Token()
        {
            Id = 1,
            CreationDate = DateTime.Now,
            ExpirationDate = DateTime.Now.AddDays(1),
            Value = "newRefreshToken"
        };
        var expectedResponse = new RefreshTokenResponse(RefreshTokenStatus.Ok, newLoginToken, newRefreshToken.Value);

        _mockLoginRepository
            .Setup(x => x.GetRefreshTokenData(request.RefreshToken, CancellationToken.None))
            .ReturnsAsync(refreshTokenData);
        _mockIdentityService
            .Setup(x => x.VerifyAccess(refreshTokenData.UserId))
            .Returns(true);
        _mockTokenService
            .Setup(x => x.GenerateLoginToken(refreshTokenData.UserId, refreshTokenData.LoginId, Enum.GetName(typeof(Role), refreshTokenData.RoleId)))
            .ReturnsAsync(newLoginToken);
        _mockTokenService
            .Setup(x => x.GenerateRefreshToken())
            .ReturnsAsync(newRefreshToken);
        _mockLoginRepository
            .Setup(x => x.RefreshToken(refreshTokenData.LoginId, newRefreshToken, CancellationToken.None));

        // when
        var handler = new RefreshTokenRequestHandler(
            _mockTokenService.Object,
            _mockLoginRepository.Object,
            _mockLogger.Object);
        var result = await handler.Handle(new RefreshTokenMediatr() { Request = request}, CancellationToken.None);

        // then
        result.Should().BeEquivalentTo(expectedResponse);

        _mockLoginRepository.Verify(x => x.RefreshToken(
            It.IsAny<long>(),
            It.IsAny<Token>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

    }

    [Fact]
    public async Task ShouldReturnAccountNotExistErrorTest()
    {
        // given
        var request = new RefreshTokenRequest()
        {
            RefreshToken = "RefreshToken"
        };
        var refreshTokenData = new RefreshTokenDto()
        {
            LoginId = 1,
            UserId = 1,
            RoleId = 2,
            AccountStatusId = 2,
            RefreshTokenId = 1,
            RefreshToken = new Token()
            {
                Id = 1,
                CreationDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddDays(1),
                Value = "RefreshToken"
            }
        };
        var expectedResponse = new RefreshTokenResponse(RefreshTokenStatus.AccountNotExist);

        _mockLoginRepository
            .Setup(x => x.GetRefreshTokenData(request.RefreshToken, CancellationToken.None))
            .ReturnsAsync((RefreshTokenDto)null);

        // when
        var handler = new RefreshTokenRequestHandler(
            _mockTokenService.Object,
            _mockLoginRepository.Object,
            _mockLogger.Object);
        var result = await handler.Handle(new RefreshTokenMediatr() { Request = request }, CancellationToken.None);

        // then
        result.Should().BeEquivalentTo(expectedResponse);

        _mockLoginRepository.Verify(x => x.GetRefreshTokenData(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()
        ), Times.Once);

        _mockTokenService.Verify(x => x.GenerateLoginToken(
            It.IsAny<long>(),
            It.IsAny<long>(),
            It.IsAny<string>()
        ), Times.Never);
    }
}
