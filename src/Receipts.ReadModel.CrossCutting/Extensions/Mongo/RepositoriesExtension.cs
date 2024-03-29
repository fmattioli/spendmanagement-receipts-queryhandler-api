using Receipts.ReadModel.Data.Queries.Repositories;
using Receipts.ReadModel.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Extensions.Mongo
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
