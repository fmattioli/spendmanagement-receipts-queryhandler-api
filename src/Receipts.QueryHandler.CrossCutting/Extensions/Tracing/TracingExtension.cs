using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Receipts.QueryHandler.Application.Constants;
using Receipts.QueryHandler.CrossCutting.Config;

namespace Receipts.QueryHandler.CrossCutting.Extensions.Tracing
{
    public static class TracingExtension
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, TracingSettings? tracing)
        {
            services.AddOpenTelemetry().WithTracing(tcb =>
            {
                tcb
                .AddSource(QueryHandlerConstants.ApplicationName)
                .SetResourceBuilder(
                    ResourceBuilder
                    .CreateDefault()
                    .AddService(serviceName: QueryHandlerConstants.ApplicationName))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(tracing?.Uri + ":" + tracing?.Port);
                    opt.Protocol = OtlpExportProtocol.Grpc;
                });
            });

            services.AddSingleton(TracerProvider.Default.GetTracer(QueryHandlerConstants.ApplicationName));
            return services;
        }
    }
}
