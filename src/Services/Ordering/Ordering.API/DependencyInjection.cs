namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AppApiServices(this IServiceCollection services)
    {
        //serives.AddCarter();

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        //app.MapCarter();

        return app;
    }
}
