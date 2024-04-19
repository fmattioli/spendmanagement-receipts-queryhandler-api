using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters;

namespace Receipts.QueryHandler.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter);
    }
}
