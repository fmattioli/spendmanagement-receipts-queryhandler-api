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
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly IMongoCollection<Receipt> receiptCollection;

        public ReceiptRepository(IMongoDatabase mongoDb)
        {
            receiptCollection = mongoDb.GetCollection<Receipt>("Receipts");
        }

        public async Task<PagedResultFilter<Receipt>> GetReceiptsAsync(ReceiptFilters queryFilter)
        {
            var results = await BuildAndExecutePipeline(queryFilter);

            return new PagedResultFilter<Receipt>
            {
                Results = results,
                PageNumber = queryFilter.PageNumber,
                PageSizeLimit = queryFilter.PageSize,
                TotalResults = results.Count(),
            };
        }

        private async Task<IEnumerable<Receipt>> BuildAndExecutePipeline(ReceiptFilters queryFilter)
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
    }
}
