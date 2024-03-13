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
    public class CategoryRepository(IMongoDatabase mongoDb) : ICategoryRepository
    {
        private readonly IMongoCollection<Category> _categoryCollection = mongoDb.GetCollection<Category>("Categories");

        public async Task<PagedResultFilter<Category>> GetCategoriesAsync(CategoryFilters queryFilter)
        {
            var resultsFiltered = await GetResultsAsync(queryFilter);
            var totaResults = await GetTotalResultsCountAsync(queryFilter);

            var aggregateCountResult = totaResults?.Count ?? 0;

            return new PagedResultFilter<Category>
            {
                Results = resultsFiltered,
                PageSize = queryFilter.PageSize,
                TotalResults = (int)aggregateCountResult
            };
        }

        private async Task<IEnumerable<Category>> GetResultsAsync(CategoryFilters queryFilter)
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

            var aggregation = await _categoryCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        private async Task<AggregateCountResult> GetTotalResultsCountAsync(CategoryFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<Category>()
                .As<Category, Category, BsonDocument>()
                .FilterCategories(queryFilter);

            PipelineDefinition<Category, AggregateCountResult> totalResultsCountPipeline;

            totalResultsCountPipeline = pipelineDefinition.Count();

            var aggregation = await this._categoryCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            return await aggregation.FirstOrDefaultAsync();
        }
    }
}
