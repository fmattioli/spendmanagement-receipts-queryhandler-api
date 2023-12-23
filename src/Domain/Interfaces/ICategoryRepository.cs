using Domain.Entities;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;

namespace Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter);
    }
}
