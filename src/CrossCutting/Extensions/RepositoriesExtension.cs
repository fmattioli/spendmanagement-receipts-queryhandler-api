using Data.Queries.Repositories;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Extensions
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IReceiptRepository, ReceiptRepository>();
            return services;
        }
    }
}
