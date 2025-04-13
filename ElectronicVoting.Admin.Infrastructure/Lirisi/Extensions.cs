using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Infrastructure.Lirisi;

public static class Extensions
{
    public static void AddWrappers(this IServiceCollection services)
    {
        services.AddTransient<LirisiWrapper>();
    }
}