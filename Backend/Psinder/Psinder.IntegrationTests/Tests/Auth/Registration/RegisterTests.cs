using FluentAssertions;
using Psinder.API.Auth.Models;
using Psinder.API.Auth.Models.Registrations;
using Psinder.DB.Auth.Entities;
using System.Net;
using System.Net.Http.Json;

namespace Psinder.IntegrationTests.Tests.Auth.Registration;

public class RegisterTests : IDisposable
{
    private readonly Sut _sut = new Sut().Start().GetAwaiter().GetResult();
    private HttpClient _client => _sut.CreateDefaultClient();
    private Fixtures _fixtures => _sut.Fixtures;

    public void Dispose()
    {
        _sut.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task CanRegisterCorrectlyTest()
    {
        // given
        var request = ReturnCommonRequest();

        //when
        var httpResult = await _client.PostAsync("https://localhost:7087/psinder/registration/register", JsonContent.Create(request));

        //then
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await httpResult.GetResponse<AuthResponse>();
        response.Should().NotBeNull();
        response!.ErrorMessage.Should().BeNull();
        response!.Status.Should().Be(AuthStatus.Ok);
    }

    [Fact]
    public async Task InvalidEmailWhileRegisterTest()
    {
        // given
        var request = ReturnCommonRequest();
        request.Email = "email";

        //when
        var httpResult = await _client.PostAsync("https://localhost:7087/psinder/registration/register", JsonContent.Create(request));

        //then
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await httpResult.GetResponse<AuthResponse>();
        response.Should().NotBeNull();
        response!.ErrorMessage.Should().BeNull();
        response!.Status.Should().Be(AuthStatus.InvalidEmail);
    }

    [Fact]
    public async Task InvalidPasswordWhileRegisterTest()
    {
        // given
        var request = ReturnCommonRequest();
        request.Password = "Pass";

        //when
        var httpResult = await _client.PostAsync("https://localhost:7087/psinder/registration/register", JsonContent.Create(request));

        //then
        httpResult.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await httpResult.GetResponse<AuthResponse>();
        response.Should().NotBeNull();
        response!.ErrorMessage.Should().NotBeNullOrEmpty();
        response!.Status.Should().Be(AuthStatus.RegistrationInvalidPassword);
    }

    private RegistrationRequest ReturnCommonRequest()
    {
        return new RegistrationRequest()
        {
            Email = "email@gmail.com",
            Password = "Password1@",
            RepeatPassword = "Password1@",
            Role = Role.Admin,
            SignedForNewsletter = true,
            UserDetails = null
        };
    }
}
