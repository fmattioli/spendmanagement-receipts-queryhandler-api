using Domain.Entities;
using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.PipelineStages
{
    internal static class FilterReceiptItemsStage
    {
        internal static PipelineDefinition<Receipt, BsonDocument> FilterReceiptItems(
            this PipelineDefinition<Receipt, BsonDocument> pipelineDefinition, GetReceiptsFilter queryFilter)
        {
            var configurationsFilter = BuildMatchFilter(queryFilter);

            if (configurationsFilter == default)
            {
                return pipelineDefinition;
            }

            var filteredConfigurations = new BsonDocument(
                "$addFields",
                new BsonDocument("ReceiptItems", configurationsFilter));

            return pipelineDefinition.AppendStage<Receipt, BsonDocument, BsonDocument>(filteredConfigurations);
        }

        private static BsonDocument BuildMatchFilter(GetReceiptsFilter queryFilter)
        {
            var filterConditions = new List<BsonDocument>
            {
                MatchByReceiptItemsIds(queryFilter)
            };

            filterConditions.RemoveAll(x => x == new BsonDocument());

            var cond = filterConditions.Count == 1
                ? filterConditions.First()
                : new BsonDocument("$and", new BsonArray(filterConditions));

            var configurationFilter = new BsonDocument
            {
                {
                    "$filter",
                    new BsonDocument
                    {
                        { "input", "$ReceiptItems" },
                        { "as", "receiptItems" },
                        { "cond", cond },
                    }
                }
            };

            return configurationFilter;
        }

        private static BsonDocument MatchByReceiptItemsIds(
            GetReceiptsFilter queryFilter)
        {
            if (!queryFilter.ReceiptItemIds.Any())
            {
                return new();
            }

            var receiptItemIds = queryFilter.ReceiptItemIds
                .Select(x => x.ToString());

            var receiptIds = new BsonDocument(
                "_id",
                new BsonDocument("$in", new BsonArray(receiptItemIds)));

            return new BsonDocument(receiptIds);
        }
    }
}
