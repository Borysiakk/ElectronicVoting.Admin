using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElectronicVoting.Admin.Infrastructure.EntityFramework;

public static class Extensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddMiniProfiler(options =>
        {
            options.RouteBasePath = "/profiler";
        }).AddEntityFramework();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new Exception("Connection string 'DefaultConnection' is missing or empty.");
        
        service.AddDbContext<ElectionDbContext>(option => option.UseSqlServer(connectionString));
        service.AddTransient<IUnitOfWork, UnitOfWork>();
        return service;
    }
}