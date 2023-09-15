using API.Extensions;
using Crosscutting.Cofig;
using CrossCutting.Extensions.HealthCheckers;
using CrossCutting.Extensions.Logging;
using CrossCutting.Extensions.Mongo;
using CrossCutting.Extensions.Tracing;
using CrossCutting.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddFilter("Microsoft", LogLevel.Critical);
});

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
builder.Services
    .AddTracing()
    .AddDependencyInjection()
    .AddRepositories()
    .AddMongo(applicationSettings.MongoSettings)
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