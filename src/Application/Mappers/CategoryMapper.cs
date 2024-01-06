using Application.Queries.Category.GetCategories;
using Domain.Entities;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using SpendManagement.WebContracts.Category;

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

        public static GetCategoriesResponse ToResponse(this PagedResultFilter<Category> categories)
        {
            return new GetCategoriesResponse
            {
                PageNumber = categories.PageNumber,
                PageSize = categories.PageSize,
                PageSizeLimit = categories.PageSizeLimit,
                Results = categories.Results.SelectMany(x => x.ToCategoryResponseItems()),
                TotalPages = categories.TotalPages,
                TotalResults = categories.TotalResults
            };
        }

        public static IEnumerable<CategoryResponse> ToCategoryResponseItems(this Category category)
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
