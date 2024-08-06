using Receipts.QueryHandler.CrossCutting.Config;
using Receipts.QueryHandler.CrossCutting.Extensions.Api;
using Receipts.QueryHandler.CrossCutting.Extensions.Auth;
using Receipts.QueryHandler.CrossCutting.Extensions.Handlers;
using Receipts.QueryHandler.CrossCutting.Extensions.HealthCheckers;
using Receipts.QueryHandler.CrossCutting.Extensions.Logging;
using Receipts.QueryHandler.CrossCutting.Extensions.MediatR;
using Receipts.QueryHandler.CrossCutting.Extensions.Mongo;
using Receipts.QueryHandler.CrossCutting.Extensions.Repositories;
using Receipts.QueryHandler.CrossCutting.Extensions.Swagger;
using Receipts.QueryHandler.CrossCutting.Extensions.Tracing;

var builder = WebApplication.CreateBuilder(args);

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

builder.Configuration
    .AddJsonFile("appsettings.json", false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", true, reloadOnChange: true)
    .AddEnvironmentVariables();


builder.Logging
    .ClearProviders()
    .AddFilter("Microsoft", LogLevel.Warning)
    .AddFilter("Microsoft", LogLevel.Critical);

var applicationSettings = builder.Configuration.GetApplicationSettings(builder.Environment);

// Add services to the container.
builder.Services
    .AddSingleton<ISettings>(applicationSettings!)
    .AddApiAuthentication(applicationSettings.AuthSettings!)
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddTracing(applicationSettings!.TracingSettings)
    .AddDependencyInjection()
    .AddRepositories()
    .AddMongo(applicationSettings.MongoSettings!)
    .AddHealthCheckers(applicationSettings)
    .AddLoggingDependency()
    .AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwagger(applicationSettings!.AuthSettings!);

var app = builder.Build();

app.UseExceptionHandler()
   .UseSwagger()
   .UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendManagement.QueryHandler.Api");
       c.OAuthClientId(applicationSettings!.AuthSettings!.ClientId);
       c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
   })
   .UseHealthCheckers()
   .UseHttpsRedirection()
   .UseAuthentication()
   .UseAuthorization();

app.MapControllers();

app.Run();