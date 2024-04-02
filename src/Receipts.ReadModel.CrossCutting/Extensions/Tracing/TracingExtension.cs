using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using Receipts.ReadModel.Application.Constants;
using Receipts.ReadModel.CrossCutting.Config;

namespace Receipts.ReadModel.CrossCutting.Extensions.Tracing
{
    public static class TracingExtension
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, TracingSettings? tracing)
        {
            services.AddOpenTelemetry().WithTracing(tcb =>
            {
                tcb
                .AddSource(ReadModelConstants.ApplicationName)
                .SetResourceBuilder(
                    ResourceBuilder
                    .CreateDefault()
                    .AddService(serviceName: ReadModelConstants.ApplicationName))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(tracing?.Uri + ":" + tracing?.Port);
                    opt.Protocol = OtlpExportProtocol.Grpc;
                });
            });

            services.AddSingleton(TracerProvider.Default.GetTracer(ReadModelConstants.ApplicationName));
            return services;
        }
    }
}
