using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Application.Services;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection service)
    {
        service.AddScoped<IVoterService, VoterService>();
        service.AddScoped<IAuthenticationService, AuthenticationService>();
        return service;
    }
}