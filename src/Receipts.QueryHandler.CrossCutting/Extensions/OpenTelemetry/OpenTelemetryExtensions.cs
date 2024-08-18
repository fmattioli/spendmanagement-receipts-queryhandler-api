using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Receipts.QueryHandler.Application.Constants;
using Receipts.QueryHandler.CrossCutting.Config;

namespace Receipts.QueryHandler.CrossCutting.Extensions.Tracing
{
    public static class OpenTelemetryExtensions
    {
        public static IServiceCollection AddTelemetry(this IServiceCollection services, MltConfigsSettings mltSettings)
        {
            var serviceName = QueryHandlerConstants.ApplicationName;
            var otelExporterEndpoint = mltSettings.GrafanaLokiUrl;

            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .UseOtlpExporter(OtlpExportProtocol.Grpc, new Uri(otelExporterEndpoint!))
                .WithTracing(builder =>
                {
                    builder
                        .AddSource(QueryHandlerConstants.ApplicationName, QueryHandlerConstants.MongoInstrumentationName)
                        .AddAspNetCoreInstrumentation();
                })
                .WithMetrics(builder =>
                {
                    builder
                        .AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation()
                        .AddProcessInstrumentation();
                });

            services.AddSingleton(TracerProvider.Default.GetTracer(QueryHandlerConstants.ApplicationName));

            return services;
        }
    }
}
