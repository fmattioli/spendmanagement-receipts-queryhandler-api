using API.Extensions;
using Crosscutting.Cofig;
using CrossCutting.Extensions;
using CrossCutting.Extensions.Logging;
using CrossCutting.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((_, config) =>
{
    var currentDirectory = Directory.GetCurrentDirectory();
    config
        .SetBasePath(currentDirectory)
        .AddJsonFile($"{currentDirectory}/appsettings.json");
}).ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddFilter("Microsoft", LogLevel.Critical);
});

var applicationSettings = builder.Configuration.GetSection("Settings").Get<Settings>();

// Add services to the container.
builder.Services
    .AddDependencyInjection()
    .AddRepositories()
    .AddMongo(applicationSettings.MongoSettings)
    .AddLoggingDependency()
    .AddControllers();

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(applicationSettings.TokenAuth)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpendManagement ReadModel", Version = "v1" });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please insert token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
        c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SpendManagement.ReadModel.xml"));
    });

var app = builder.Build();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpendManagement.ReadModel"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
