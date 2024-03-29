using Application.Queries.Category.GetCategories;
using Receipts.ReadModel;
using Receipts.ReadModel.Entities;
using Receipts.ReadModel.QueriesFilters;
using Receipts.ReadModel.QueriesFilters.PageFilters;
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
                (short)getCategoriesRequest.PageFilter.PageNumber,
                (short)getCategoriesRequest.PageFilter.PageSize);
        }

        public static GetCategoriesResponse ToResponse(this PagedResultFilter<Category> categories, PageFilterRequest pageFilter)
        {
            return new GetCategoriesResponse
            {
                PageNumber = pageFilter.PageNumber,
                PageSize = pageFilter.PageSize,
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
