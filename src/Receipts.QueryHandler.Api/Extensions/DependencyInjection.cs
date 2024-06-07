using Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts;

namespace Receipts.QueryHandler.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddMediatR(
                    x => x.RegisterServicesFromAssemblies(
                        typeof(GetVariableReceiptsQuery).Assembly));

            return services;
        }
    }
}
