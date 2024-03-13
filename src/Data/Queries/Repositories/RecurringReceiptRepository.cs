using Data.Queries.PipelineStages;
using Data.Queries.PipelineStages.RecurringReceipt;
using Domain.Entities;
using Domain.Interfaces;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.Repositories
{
    public class RecurringReceiptRepository(IMongoDatabase mongoDb) : IRecurringReceiptRepository
    {
        private readonly IMongoCollection<RecurringReceipt> recurringReceiptCollection = mongoDb.GetCollection<RecurringReceipt>("RecurringReceipts");

        public async Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter)
        {
            var filteredResults = await GetResultsAsync(queryFilter);
            var totaResults = await GetTotalResultsCountAsync(queryFilter);

            var aggregateCountResult = totaResults?.Count ?? 0;

            return new PagedResultFilter<RecurringReceipt>
            {
                PageSize = queryFilter.PageSize,
                Results = filteredResults,
                TotalResults = (int)aggregateCountResult
            };
        }

        private async Task<IEnumerable<RecurringReceipt>> GetResultsAsync(RecurringReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                            .For<RecurringReceipt>()
                            .As<RecurringReceipt, RecurringReceipt, BsonDocument>()
                            .FilterRecurringReceipts(queryFilter)
                            .Paginate(queryFilter.PageSize, queryFilter.PageNumber)
                            .Sort(
                                Builders<BsonDocument>.Sort.Ascending(
                                    new StringFieldDefinition<BsonDocument>(
                                        nameof(Receipt.Id))));

            var resultsPipeline = pipelineDefinition.As<RecurringReceipt, BsonDocument, RecurringReceipt>();

            var aggregation = await recurringReceiptCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        private async Task<AggregateCountResult> GetTotalResultsCountAsync(RecurringReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<RecurringReceipt>()
                .As<RecurringReceipt, RecurringReceipt, BsonDocument>()
                .FilterRecurringReceipts(queryFilter);

            PipelineDefinition<RecurringReceipt, AggregateCountResult> totalResultsCountPipeline;

            totalResultsCountPipeline = pipelineDefinition.Count();

            var aggregation = await this.recurringReceiptCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            return await aggregation.FirstOrDefaultAsync();
        }
    }
}
