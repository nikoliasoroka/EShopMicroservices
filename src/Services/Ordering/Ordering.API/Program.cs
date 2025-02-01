using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AppApplicationServices()
    .AppInfrastructureServices(builder.Configuration)
    .AppApiServices();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.Run();
