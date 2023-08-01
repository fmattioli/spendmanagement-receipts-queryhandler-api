using Domain.Entities;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;

namespace Domain.Interfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter);
    }
}
