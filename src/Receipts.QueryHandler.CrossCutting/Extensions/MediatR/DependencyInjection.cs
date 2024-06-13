using Microsoft.Extensions.DependencyInjection;
using Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts;

namespace Receipts.QueryHandler.CrossCutting.Extensions.MediatR
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
