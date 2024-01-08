using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Psinder.DB.Auth.Repositories.Logins;
using Psinder.DB.Common.Repositories.Configurations;
using Psinder.DB.Common.Repositories.Files;
using Psinder.DB.Common.Repositories.UnitOfWorks;
using Psinder.DB.Common.Services;
using Psinder.DB.Domain.Repositories.Appointments;
using Psinder.DB.Domain.Repositories.Pets;
using Psinder.DB.Domain.Repositories.Shelters;
using Psinder.DB.Domain.Repositories.Users;

namespace Psinder.DB;

public static class RepositoriesExtensions
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection service, IConfiguration configuration)
    {
        service.ConfigureDatabase(configuration);
        service.AddScoped<DatabaseContext>();
        service.AddScoped<IUnitOfWork, UnitOfWork>();

        service.AddTransient<ILoginRepository, LoginRepository>();
        service.AddTransient<IUserRepository, UserRepository>();
        service.AddTransient<IShelterRepository, ShelterRepository>();
        service.AddTransient<IAppointmentRepository, AppointmentRepository>();
        service.AddTransient<IPetRepository, PetRepository>();

        service.AddTransient<IConfigurationRepository, ConfigurationRepository>();
        service.AddTransient<IFileRepository, FileRepository>();
        service.AddTransient<ICacheService, CacheService>();

        return service;
    }
}