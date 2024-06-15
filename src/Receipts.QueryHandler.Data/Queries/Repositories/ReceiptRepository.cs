using MongoDB.Bson;
using MongoDB.Driver;
using Receipts.QueryHandler.Data.Queries.PipelineStages;
using Receipts.QueryHandler.Data.Queries.PipelineStages.Receipt;
using Receipts.QueryHandler.Data.Queries.PipelineStages.RecurringReceipt;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.Interfaces;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Data.Queries.Repositories
{
    public class ReceiptRepository(IMongoDatabase mongoDb) : IReceiptRepository
    {
        private readonly IMongoCollection<VariableReceipt> receiptCollection = mongoDb.GetCollection<VariableReceipt>("VariableReceipts");
        private readonly IMongoCollection<RecurringReceipt> recurringReceiptCollection = mongoDb.GetCollection<RecurringReceipt>("RecurringReceipts");


        #region Variable receipts
        public async Task<PagedResultFilter<VariableReceipt>> GetVariableReceiptsAsync(ReceiptFilters queryFilter)
        {
            var filteredResults = FindVariableReceiptsResultsAsync(queryFilter);
            var totaResults = GetVariableReceiptsTotalResultsAsync(queryFilter);
            var receiptsTotal = GetVariableReceiptsTotalAmount(queryFilter);

            await Task.WhenAll(filteredResults, totaResults, receiptsTotal);

            return new PagedResultFilter<VariableReceipt>
            {
                PageSize = queryFilter.PageSize,
                Results = await filteredResults,
                ReceiptsTotalAmount = await receiptsTotal,
                TotalResults = (int)await totaResults
            };
        }

        private async Task<IEnumerable<VariableReceipt>> FindVariableReceiptsResultsAsync(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                            .For<VariableReceipt>()
                            .As<VariableReceipt, VariableReceipt, BsonDocument>()
                            .FilterReceipts(queryFilter)
                            .FilterReceiptItems(queryFilter)
                            .Sort(
                                Builders<BsonDocument>.Sort.Descending(
                                    new StringFieldDefinition<BsonDocument>(
                                        nameof(VariableReceipt.ReceiptDate))))
                            .Paginate(queryFilter.PageSize, queryFilter.PageNumber);

            var resultsPipeline = pipelineDefinition.As<VariableReceipt, BsonDocument, VariableReceipt>();

            var aggregation = await receiptCollection.AggregateAsync(
                                  resultsPipeline,
                                  new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        private async Task<long> GetVariableReceiptsTotalResultsAsync(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<VariableReceipt>()
                .As<VariableReceipt, VariableReceipt, BsonDocument>()
                .FilterReceipts(queryFilter)
                .FilterReceiptItems(queryFilter);

            PipelineDefinition<VariableReceipt, AggregateCountResult> totalResultsCountPipeline;

            totalResultsCountPipeline = pipelineDefinition.Count();

            var aggregation = await receiptCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            var totaResults = await aggregation.FirstOrDefaultAsync();
            return totaResults?.Count ?? 0;
        }

        private async Task<decimal> GetVariableReceiptsTotalAmount(ReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<VariableReceipt>()
                .As<VariableReceipt, VariableReceipt, BsonDocument>()
                .FilterReceipts(queryFilter)
                .FilterReceiptItems(queryFilter)
                .MakeSumBasedOnFilterName(nameof(VariableReceipt.Total));

            var aggregateOptions = new AggregateOptions { AllowDiskUse = true };

            var aggregation = await receiptCollection.AggregateAsync(pipelineDefinition, aggregateOptions);

            var document = await aggregation.FirstOrDefaultAsync();

            if (document != null && document.Contains(nameof(VariableReceipt.Total)))
                return document[nameof(VariableReceipt.Total)].AsDecimal;

            return 0;
        }
        #endregion

        #region Recurring Receipts
        public async Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter)
        {
            var filteredResults = FindRecurringReceiptsResultsAsync(queryFilter);
            var totalResults = GetRecurringReceiptsTotalResultsAsync(queryFilter);
            var totalAmount = GetRecurringReceiptsTotalAmount(queryFilter);

            await Task.WhenAll(filteredResults, totalResults, totalAmount);

            return new PagedResultFilter<RecurringReceipt>
            {
                PageSize = queryFilter.PageSize,
                Results = await filteredResults,
                TotalResults = (int)await totalResults,
                ReceiptsTotalAmount = await totalAmount,
            };
        }

        private async Task<IEnumerable<RecurringReceipt>> FindRecurringReceiptsResultsAsync(RecurringReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<RecurringReceipt>()
                .As<RecurringReceipt, RecurringReceipt, BsonDocument>()
                .FilterRecurringReceipts(queryFilter)
                .Sort(
                    Builders<BsonDocument>.Sort.Descending(
                        new StringFieldDefinition<BsonDocument>(
                            nameof(VariableReceipt.Id))))
                .Paginate(queryFilter.PageSize, queryFilter.PageNumber);

            var resultsPipeline = pipelineDefinition.As<RecurringReceipt, BsonDocument, RecurringReceipt>();

            var aggregation = await recurringReceiptCollection.AggregateAsync(
                resultsPipeline,
                new AggregateOptions { AllowDiskUse = true, MaxTime = Timeout.InfiniteTimeSpan, });

            return await aggregation.ToListAsync();
        }

        protected async Task<long> GetRecurringReceiptsTotalResultsAsync(RecurringReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
                .For<RecurringReceipt>()
                .As<RecurringReceipt, RecurringReceipt, BsonDocument>()
                .FilterRecurringReceipts(queryFilter);

            PipelineDefinition<RecurringReceipt, AggregateCountResult> totalResultsCountPipeline;

            totalResultsCountPipeline = pipelineDefinition.Count();

            var aggregation = await recurringReceiptCollection.AggregateAsync(
                                  totalResultsCountPipeline,
                                  new AggregateOptions { AllowDiskUse = true });

            var totaResults = await aggregation.FirstOrDefaultAsync();
            return totaResults?.Count ?? 0;
        }

        private async Task<decimal> GetRecurringReceiptsTotalAmount(RecurringReceiptFilters queryFilter)
        {
            var pipelineDefinition = PipelineDefinitionBuilder
               .For<RecurringReceipt>()
               .As<RecurringReceipt, RecurringReceipt, BsonDocument>()
               .FilterRecurringReceipts(queryFilter)
               .MakeSumBasedOnFilterName(nameof(RecurringReceipt.RecurrenceTotalPrice));

            var aggregateOptions = new AggregateOptions { AllowDiskUse = true };

            var aggregation = await recurringReceiptCollection.AggregateAsync(pipelineDefinition, aggregateOptions);

            var document = await aggregation.FirstOrDefaultAsync();

            if (document != null && document.Contains(nameof(RecurringReceipt.RecurrenceTotalPrice)))
                return document[nameof(RecurringReceipt.RecurrenceTotalPrice)].AsDecimal;

            return 0;
        }
        #endregion
    }
}
