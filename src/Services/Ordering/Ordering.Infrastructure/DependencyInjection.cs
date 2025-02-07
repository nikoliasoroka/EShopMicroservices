using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AppInfrastructureServices
        (this IServiceCollection services, IConfiguration configuration)
    {
        var connectingString = configuration.GetConnectionString("Database");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectingString));

        return services;
    }
}