using Data.Queries.PipelineStages;
using Data.Queries.PipelineStages.Receipt;
using Data.Queries.PipelineStages.RecurringReceipt;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.Repositories
{
    public class ReceiptRepository(IMongoDatabase mongoDb) : IReceiptRepository
    {
        private readonly IMongoCollection<Receipt> receiptCollection = mongoDb.GetCollection<Receipt>("Receipts");
        private readonly IMongoCollection<RecurringReceipt> recurringReceiptCollection = mongoDb.GetCollection<RecurringReceipt>("RecurringReceipts");

        public async Task<PagedResultFilter<Receipt>> GetVariableReceiptsAsync(ReceiptFilters queryFilter)
        {
            var filteredResults = await FindVariableReceiptsResultsAsync(queryFilter);
            var totaResults = await GetTotalResultsCountAsync(queryFilter);
            var receiptsTotal = await GetReceiptsTotalAmount(queryFilter);


            return new PagedResultFilter<Receipt>
            {
                PageSize = queryFilter.PageSize,
                Results = filteredResults,
                ReceiptsTotalAmount = receiptsTotal,
                TotalResults = (int)totaResults
            };
        }

        public async Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter)
        {
            var filteredResults = await FindRecurringReceiptsResultsAsync(queryFilter);
            var totaResults = await GetTotalResultsCountAsync(queryFilter);

            var aggregateCountResult = totaResults?.Count ?? 0;

            return new PagedResultFilter<RecurringReceipt>
            {
                PageSize = queryFilter.PageSize,
                Results = filteredResults,
                TotalResults = (int)aggregateCountResult
            };
        }

        private async Task<IEnumerable<RecurringReceipt>> FindRecurringReceiptsResultsAsync(RecurringReceiptFilters queryFilter)
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

        protected async Task<AggregateCountResult> GetTotalResultsCountAsync(RecurringReceiptFilters queryFilter)
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

        private async Task<IEnumerable<Receipt>> FindVariableReceiptsResultsAsync(ReceiptFilters queryFilter)
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
                                        nameof(Receipt.ReceiptDate))));

            var resultsPipeline = pipelineDefinition.As<Receipt, BsonDocument, Receipt>();

            var aggregation = await receiptCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        private async Task<long> GetTotalResultsCountAsync(ReceiptFilters queryFilter)
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

            var totaResults = await aggregation.FirstOrDefaultAsync();
            return totaResults?.Count ?? 0;
        }

        private async Task<decimal> GetReceiptsTotalAmount(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<Receipt>()
                .As<Receipt, Receipt, BsonDocument>()
                .FilterReceipts(queryFilter)
                .FilterReceiptItems(queryFilter)
                .MakeSumTotalReceipts();

            var aggregateOptions = new AggregateOptions { AllowDiskUse = true };

            var aggregation = await receiptCollection.AggregateAsync(pipelineDefinition, aggregateOptions);

            var document = await aggregation.FirstOrDefaultAsync();

            return document["total"].AsDecimal;
        }
    }
}
