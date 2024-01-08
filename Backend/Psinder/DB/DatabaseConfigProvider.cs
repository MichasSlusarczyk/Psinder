using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Psinder.DB;

public static class DatabaseConfigProvider
{
    public static IServiceCollection ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseConfig config = new DatabaseConfig()
        {
            Type = "MAIN",
            ConnesctionString = configuration.GetConnectionString("DB")
        };

        services.AddSingleton(Options.Create(config));

        return services;
    }
}
