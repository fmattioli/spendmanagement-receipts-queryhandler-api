using Receipts.QueryHandler.Api.Extensions;
using Receipts.QueryHandler.CrossCutting.Extensions.Logging;
using Receipts.QueryHandler.CrossCutting.Extensions.Mongo;
using Receipts.QueryHandler.CrossCutting.Extensions.HealthCheckers;
using Receipts.QueryHandler.CrossCutting.Extensions.Tracing;
using Receipts.QueryHandler.CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);

builder.Logging
    .ClearProviders()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft", LogLevel.Critical);

// Add services to the container.
builder.Services
    .AddTracing(applicationSettings!.TracingSettings)
    .AddDependencyInjection()
    .AddRepositories()
    .AddMongo(applicationSettings.MongoSettings!)
    .AddAuthorization(applicationSettings.TokenAuth)
    .AddHealthCheckers(applicationSettings)
    .AddLoggingDependency()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendManagement.ReadModel"));
app.UseHealthCheckers();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();