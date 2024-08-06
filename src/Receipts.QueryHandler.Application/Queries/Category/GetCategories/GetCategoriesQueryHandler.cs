using Feijuca.Keycloak.MultiTenancy.Services;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Category.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IAuthService authService)
                : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;
        private readonly IAuthService _authService = authService;
        public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var tenantId = int.Parse(_authService.GetTenantFromToken());
            Guid userId = _authService.GetUserIdFromToken();

            var domainFilters = request.GetReceiptsRequest.ToDomainFilters(userId, tenantId);
            var categoriesQueryResult = await _categoryRepository.GetCategoriesAsync(domainFilters);
            return categoriesQueryResult.ToResponse(request.GetReceiptsRequest.PageFilter);
        }
    }
}
