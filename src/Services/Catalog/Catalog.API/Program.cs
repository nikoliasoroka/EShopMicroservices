var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var assembly = typeof(Program).Assembly;
var databaseConnection = builder.Configuration.GetConnectionString("Database");

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(databaseConnection!);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnection!);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapCarter();

app.UseExceptionHandler(option => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
