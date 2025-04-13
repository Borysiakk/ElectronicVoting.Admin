using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Infrastructure.Paillier;

public static class Extensions
{
    public static IServiceCollection AddPaillier(this IServiceCollection service)
    {
        service.AddScoped<IPaillierService, PaillierService>();
        return service;
    }
}