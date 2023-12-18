using Application.Queries.Category.GetCategories;
using Domain.Entities;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using Web.Contracts.Category;

namespace Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                CreatedDate = category.CreatedDate,
                Name = category.Name,
            };
        }

        public static CategoryFilters ToDomainFilters(this GetCategoriesRequest getCategoriesRequest)
        {
            return new CategoryFilters(
                getCategoriesRequest.CategoryIds,
                getCategoriesRequest.CategoryNames,
                (short)getCategoriesRequest.PageFilter.Page,
                (short)getCategoriesRequest.PageFilter.PageSize);
        }

        public static GetCategoriesResponse ToResponse(this PagedResultFilter<Category> receipts)
        {
            return new GetCategoriesResponse
            {
                PageNumber = receipts.PageNumber,
                PageSize = receipts.PageSize,
                PageSizeLimit = receipts.PageSizeLimit,
                Results = receipts.Results.SelectMany(x => x.ToResponseItems()),
                TotalPages = receipts.TotalPages,
                TotalResults = receipts.TotalResults
            };
        }

        public static IEnumerable<CategoryResponse> ToResponseItems(this Category category)
        {
            return new List<CategoryResponse>
            {
                new() {
                    Name = category.Name,
                    Id = category.Id,
                }
            };
        }
    }
}
