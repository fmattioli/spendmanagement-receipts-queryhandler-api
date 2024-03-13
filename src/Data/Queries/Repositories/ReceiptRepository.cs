using Data.Queries.PipelineStages;
using Data.Queries.PipelineStages.Receipt;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters.PageFilters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.Repositories
{
    public class ReceiptRepository(IMongoDatabase mongoDb) : IReceiptRepository
    {
        private readonly IMongoCollection<Receipt> receiptCollection = mongoDb.GetCollection<Receipt>("Receipts");

        public async Task<PagedResultFilter<Receipt>> GetReceiptsAsync(ReceiptFilters queryFilter)
        {
            var filteredResults = await GetResultsAsync(queryFilter);
            var totaResults = await GetTotalResultsCountAsync(queryFilter);

            var receiptsTotal = filteredResults.Sum(x => x.Total);
            var aggregateCountResult = totaResults?.Count ?? 0;

            return new PagedResultFilter<Receipt>
            {
                PageSize = queryFilter.PageSize,
                Results = filteredResults,
                ReceiptsTotalAmount = receiptsTotal,
                TotalResults = (int)aggregateCountResult
            };
        }

        private async Task<IEnumerable<Receipt>> GetResultsAsync(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                            .For<Receipt>()
                            .As<Receipt, Receipt, BsonDocument>()
                            .FilterReceipts(queryFilter)
                            .FilterReceiptItems(queryFilter)
                            .Paginate(queryFilter.PageSize, queryFilter.PageNumber)
                            .Sort(
                                Builders<BsonDocument>.Sort.Ascending(
                                    new StringFieldDefinition<BsonDocument>(
                                        nameof(Receipt.Id))));

            var resultsPipeline = pipelineDefinition.As<Receipt, BsonDocument, Receipt>();

            var aggregation = await receiptCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        private async Task<AggregateCountResult> GetTotalResultsCountAsync(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<Receipt>()
                .As<Receipt, Receipt, BsonDocument>()
                .FilterReceipts(queryFilter)
                .FilterReceiptItems(queryFilter);

            PipelineDefinition<Receipt, AggregateCountResult> totalResultsCountPipeline;

            totalResultsCountPipeline = pipelineDefinition.Count();

            var aggregation = await this.receiptCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            return await aggregation.FirstOrDefaultAsync();
        }
    }
}
