using Receipts.ReadModel.Interfaces;
using MediatR;
using Receipts.ReadModel.Application.Mappers;

namespace Receipts.ReadModel.Application.Queries.Category.GetCategories
{
    public class GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
                : IRequestHandler<GetCategoriesQuery, GetCategoriesResponse>
    {
        private readonly ICategoryRepository _categoryRepository = categoryRepository;

        public async Task<GetCategoriesResponse> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetReceiptsRequest.ToDomainFilters();
            var categoriesQueryResult = await _categoryRepository.GetCategoriesAsync(domainFilters);
            return categoriesQueryResult.ToResponse(request.GetReceiptsRequest.PageFilter);
        }
    }
}
