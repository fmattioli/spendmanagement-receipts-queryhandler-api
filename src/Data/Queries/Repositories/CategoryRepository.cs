using Data.Queries.PipelineStages;
using Data.Queries.PipelineStages.Category;
using Domain.Entities;
using Domain.Interfaces;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoCollection<Category> categoryCollection;

        public CategoryRepository(IMongoDatabase mongoDb)
        {
            categoryCollection = mongoDb.GetCollection<Category>("Categories");
        }

        public async Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter)
        {
            var results = await BuildAndExecutePipeline(queryFilter);

            return new PagedResultFilter<Category>
            {
                Results = results,
                PageNumber = queryFilter.PageNumber,
                PageSizeLimit = queryFilter.PageSize,
                TotalResults = results.Count(),
            };
        }

        private async Task<IEnumerable<Category>> BuildAndExecutePipeline(CategoryFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                            .For<Category>()
                            .As<Category, Category, BsonDocument>()
                            .FilterCategories(queryFilter)
                            .Paginate(queryFilter.PageSize, queryFilter.PageNumber)
                            .Sort(
                                Builders<BsonDocument>.Sort.Ascending(
                                    new StringFieldDefinition<BsonDocument>(
                                        nameof(Receipt.Id))));

            var resultsPipeline = pipelineDefinition.As<Category, BsonDocument, Category>();

            var aggregation = await categoryCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }
    }
}
