using MongoDB.Bson;
using MongoDB.Driver;
using Receipts.QueryHandler.Data.Queries.PipelineStages;
using Receipts.QueryHandler.Data.Queries.PipelineStages.Category;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.Interfaces;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Data.Queries.Repositories
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
                                        nameof(VariableReceipt.Id))));

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

            var aggregation = await _categoryCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            return await aggregation.FirstOrDefaultAsync();
        }
    }
}
