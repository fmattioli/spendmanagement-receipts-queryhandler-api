using Contracts.Web.Category.Requests;
using Contracts.Web.Category.Responses;
using Contracts.Web.Common;
using Receipts.QueryHandler.Application.Queries.Category.GetCategories;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Application.Mappers
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

        public static CategoryFilters ToDomainFilters(this GetCategoriesRequest getCategoriesRequest, int tenantId)
        {
            return new CategoryFilters(
                tenantId,
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
