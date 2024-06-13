using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Application.Providers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Category.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IAuthProvider authProvider)
                : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IAuthProvider _authProvider = authProvider;
        public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            int tenantId = _authProvider.GetTenant();
            var domainFilters = request.GetReceiptsRequest.ToDomainFilters(tenantId);
            var categoriesQueryResult = await _categoryRepository.GetCategoriesAsync(domainFilters);
            return categoriesQueryResult.ToResponse(request.GetReceiptsRequest.PageFilter);
        }
    }
}
