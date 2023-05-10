using Data.Queries.PipelineStages;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Queries;
using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Persistence.Repositories
{
    public class ReceiptRepository : BaseRepository<Receipt>, IReceiptRepository
    {
        private readonly IMongoCollection<Receipt> receiptCollection;

        public ReceiptRepository(IMongoDatabase mongoDb) : base(mongoDb, "Receipts")
        {
            this.receiptCollection = mongoDb.GetCollection<Receipt>("Receipts");
        }

        public async Task<PagedResultFilter<Receipt>> GetReceiptsAsync(GetReceiptsFilter queryFilter)
        {
            var results = await BuildAndExecutePipeline(queryFilter);

            return new PagedResultFilter<Receipt>
            {
                Results = results,
                PageNumber = queryFilter.PageFilter.PageNumber,
                PageSizeLimit = queryFilter.PageFilter.PageSize,
                TotalResults = results.Count(),
            };
        }

        private async Task<IEnumerable<Receipt>> BuildAndExecutePipeline(GetReceiptsFilter queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                            .For<Receipt>()
                            .As<Receipt, Receipt, BsonDocument>()
                            .FilterReceipts(queryFilter)
                            .FilterReceiptItems(queryFilter)
                            .Paginate(queryFilter)
                            .Sort(
                                Builders<BsonDocument>.Sort.Ascending(
                                    new StringFieldDefinition<BsonDocument>(
                                        $"{nameof(Receipt.Id)}")));

            var resultsPipeline = pipelineDefinition.As<Receipt, BsonDocument, Receipt>();

            var aggregation = await this.receiptCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }
    }
}
