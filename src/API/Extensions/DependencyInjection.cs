using Application.GetReceipts;

namespace API.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(GetReceiptsQuery).Assembly));

            return services;
        }
    }
} 
