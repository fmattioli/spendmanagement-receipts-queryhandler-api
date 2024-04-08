using Receipts.ReadModel.Application.Queries.Category.GetCategories;
using Receipts.ReadModel.Entities;
using Receipts.ReadModel.QueriesFilters;
using Receipts.ReadModel.QueriesFilters.PageFilters;
using Web.Contracts.Category.Requests;
using Web.Contracts.Category.Responses;
using Web.Contracts.Common;

namespace Receipts.ReadModel.Application.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToCategoryResponse(this Category category)
        {
            return new CategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                CreatedDate = category.CreatedDate,
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

        public static GetCategoriesResponse ToResponse(this PagedResultFilter<Category> categories, PageFilterRequest pageFilter)
        {
            return new GetCategoriesResponse
            {
                PageNumber = pageFilter.Page,
                PageSize = pageFilter.PageSize,
                Results = categories.Results.SelectMany(x => x.ToCategoryResponseItems()),
                TotalPages = categories.TotalPages,
                TotalResults = categories.TotalResults
            };
        }

        public static IEnumerable<CategoryResponse> ToCategoryResponseItems(this Category category)
        {
            return
            [
                new() {
                    Name = category.Name,
                    Id = category.Id,
                    CreatedDate = category.CreatedDate
                }
            ];
        }
    }
}
