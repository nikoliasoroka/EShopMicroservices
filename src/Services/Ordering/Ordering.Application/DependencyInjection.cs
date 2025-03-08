using BuildingBlocks.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using BuildingBlocks.Messaging.MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;

namespace Ordering.Application;

public static class DependencyInjection
{
    public static IServiceCollection AppApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        services.AddFeatureManagement();
        services.AddMessageBroker(configuration, assembly);

        return services;
    }
}