using Application.Constants;
using CrossCutting.Config;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace CrossCutting.Extensions.Tracing
{
    public static class TracingExtension
    {
        public static IServiceCollection AddTracing(this IServiceCollection services, TracingSettings? tracing)
        {
            services.AddOpenTelemetry().WithTracing(tcb =>
            {
                tcb
                .AddSource(Constants.ApplicationName)
                .SetResourceBuilder(
                    ResourceBuilder
                    .CreateDefault()
                    .AddService(serviceName: Constants.ApplicationName))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(opt =>
                {
                    opt.Endpoint = new Uri(tracing?.Uri + ":" + tracing?.Port);
                    opt.Protocol = OtlpExportProtocol.Grpc;
                });
            });

            services.AddSingleton(TracerProvider.Default.GetTracer(Constants.ApplicationName));
            return services;
        }
    }
}
