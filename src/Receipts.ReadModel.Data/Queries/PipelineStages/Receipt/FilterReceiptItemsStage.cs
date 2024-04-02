using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Receipts.ReadModel.QueriesFilters;

namespace Receipts.ReadModel.Data.Queries.PipelineStages.Receipt
{
    internal static class FilterReceiptItemsStage
    {
        internal static PipelineDefinition<Entities.Receipt, BsonDocument> FilterReceiptItems(
            this PipelineDefinition<Entities.Receipt, BsonDocument> pipelineDefinition,
            ReceiptFilters queryFilter)
        {
            var matchFilter = BuildMatchFilter(queryFilter);

            if (matchFilter != FilterDefinition<BsonDocument>.Empty)
            {
                pipelineDefinition = pipelineDefinition.Match(matchFilter);
            }

            return pipelineDefinition;
        }

        private static FilterDefinition<BsonDocument> BuildMatchFilter(ReceiptFilters queryFilter)
        {
            var filters = new List<FilterDefinition<BsonDocument>>
            {
                MatchByReceiptItemIds(queryFilter),
                MatchByItemNames(queryFilter),
            };

            filters.RemoveAll(x => x == FilterDefinition<BsonDocument>.Empty);

            if (filters.Count == 0)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            return filters.Count == 1 ? filters[0] : Builders<BsonDocument>.Filter.And(filters);
        }

        private static FilterDefinition<BsonDocument> MatchByItemNames(
            ReceiptFilters queryFilter)
        {
            if (!queryFilter.ReceiptItemNames.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var itemNames = queryFilter.ReceiptItemNames
                .Select(x => new BsonRegularExpression(new Regex(x, RegexOptions.IgnoreCase)));

            var filter = new BsonDocument(
                "ReceiptItems.ItemName",
                new BsonDocument("$in", BsonArray.Create(itemNames)));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
        }

        private static FilterDefinition<BsonDocument> MatchByReceiptItemIds(
            ReceiptFilters queryFilter)
        {
            if (!queryFilter.ReceiptItemIds.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var receiptItemIds = queryFilter.ReceiptItemIds
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptIds = new BsonDocument(
                "ReceiptItems._id",
                new BsonDocument("$in", new BsonArray(receiptItemIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptIds);
        }
    }
}
