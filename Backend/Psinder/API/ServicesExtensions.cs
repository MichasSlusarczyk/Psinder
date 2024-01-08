using Dapper;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Psinder.API.Auth.Common;
using Psinder.API.Auth.Services.Facebook;
using Psinder.API.Auth.Services.Login;
using Psinder.API.Auth.Services.Recaptcha;
using Psinder.API.Auth.Services.Registration;
using Psinder.API.Common;
using Psinder.API.Common.Filters;
using Psinder.API.Common.Helpers;
using Psinder.API.Common.Services;

namespace Psinder.API;

public static class ServicesExtensions
{
    public static IServiceCollection RegisterServices(this IServiceCollection service)
    {
        service.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        service.AddTransient<RunAsTransactionFilter>();
        service.ConfigureDapper();

        service.AddTransient<IEmailSenderService, EmailSenderService>();
        service.AddTransient<IEmailValidationService, EmailValidationService>();
        service.AddTransient<IPasswordValidationService, PasswordVaildationService>();
        service.AddTransient<ITokenService, TokenService>();
        service.AddTransient<IHashService, HashService>();
        service.AddTransient<IIdentityService, IdentityService>();
        service.AddTransient<IRecaptchaService, RecaptchaService>();
        service.AddTransient<IFacebookService, FacebookService>();
        service.AddTransient<ILoginService, LoginService>();
        service.AddTransient<IRegisterService, RegisterService>();
        service.AddTransient<ITranslatorService, TranslatorService>();

        return service;
    }

    public static IServiceCollection ConfigureDapper(this IServiceCollection service)
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        SqlMapper.AddTypeHandler(new SqlDateOnlyTypeHandler());
        SqlMapper.AddTypeHandler(new SqlTimeOnlyTypeHandler());

        return service;
    }

}