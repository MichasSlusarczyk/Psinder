using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Psinder.DB;

namespace Psinder.IntegrationTests;

public class Sut : WebApplicationFactory<Program>
{
    private readonly string _connectionString = "Server=(localdb)\\mssqllocaldb;Database=PsinderDBTest;Trusted_Connection=True;";

    private IServiceScope? _fixturesScope;

    public Fixtures Fixtures 
    { 
        get
        {
            _fixturesScope ??= Services.CreateScope();
            return _fixturesScope.ServiceProvider.GetService<Fixtures>()!;
        } 
    }

    public override async ValueTask DisposeAsync()
    {
        _fixturesScope?.Dispose();
        await base.DisposeAsync();
    }

    public async Task<Sut> Start()
    {
        return this;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
            {
                var databaseConfig = new DatabaseConfig()
                {
                    Type = "TEST",
                    ConnesctionString = _connectionString
                };

                services.AddSingleton(Options.Create(databaseConfig));
                services.AddTransient<Fixtures>();
            }
        );

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.DefaultScheme;
                options.DefaultScheme= TestAuthHandler.DefaultScheme;
            }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.DefaultScheme, options => { });

            services.RemoveAll<IDistributedCache>();
            services.AddDistributedMemoryCache();
        });
    }
}
