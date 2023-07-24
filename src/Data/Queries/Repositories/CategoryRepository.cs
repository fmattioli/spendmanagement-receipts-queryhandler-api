using Domain.Entities;
using Domain.Interfaces;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using MongoDB.Driver;

namespace Data.Queries.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IMongoCollection<Category> categoryCollection;

        public CategoryRepository(IMongoDatabase mongoDb) : base(mongoDb, "Receipts")
        {
            categoryCollection = mongoDb.GetCollection<Category>("Categories");
        }

        public Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoriesFilters queryFilter)
        {
            throw new NotImplementedException();
        }
    }
}
