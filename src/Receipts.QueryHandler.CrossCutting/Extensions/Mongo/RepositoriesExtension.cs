using Microsoft.Extensions.DependencyInjection;
using Receipts.QueryHandler.Data.Queries.Repositories;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.CrossCutting.Extensions.Mongo
{
    public static class RepositoriesExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}
