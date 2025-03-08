var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Application services
var assembly = typeof(Program).Assembly;

var connections = new
{
    Database = builder.Configuration.GetConnectionString("Database")!,
    Redis = builder.Configuration.GetConnectionString("Redis")!
};

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();

// Data services
builder.Services.AddMarten(options =>
{
    options.Connection(connections.Database);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CashedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = connections.Redis;
});

// gRPC services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});

// Async Communication Services
builder.Services.AddMessageBroker(builder.Configuration);

// Cross-cutting services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(connections.Database)
    .AddRedis(connections.Redis);

// Configure the HTTP request pipeline.
var app = builder.Build();

app.MapCarter();

app.UseExceptionHandler(option => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
