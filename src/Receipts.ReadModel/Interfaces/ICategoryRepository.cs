using Receipts.ReadModel.QueriesFilters;
using Receipts.ReadModel.QueriesFilters.PageFilters;
using Receipts.ReadModel.Entities;

namespace Receipts.ReadModel.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter);
    }
}
