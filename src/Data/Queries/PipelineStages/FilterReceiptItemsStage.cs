using Domain.Entities;
using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Data.Queries.PipelineStages
{
    internal static class FilterReceiptItemsStage
    {
        internal static PipelineDefinition<Receipt, BsonDocument> FilterReceiptItems(
            this PipelineDefinition<Receipt, BsonDocument> pipelineDefinition,
            ReceiptsFilters queryFilter)
        {
            var matchFilter = BuildMatchFilter(queryFilter);

            if (matchFilter != FilterDefinition<BsonDocument>.Empty)
            {
                pipelineDefinition = pipelineDefinition.Match(matchFilter);
            }

            return pipelineDefinition;
        }

        private static FilterDefinition<BsonDocument> BuildMatchFilter(ReceiptsFilters queryFilter)
        {
            var filters = new List<FilterDefinition<BsonDocument>>
            {
                MatchByItemNames(queryFilter),
            };

            filters.RemoveAll(x => x == FilterDefinition<BsonDocument>.Empty);

            if (!filters.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            return filters.Count == 1 ? filters.First() : Builders<BsonDocument>.Filter.And(filters);
        }

        private static FilterDefinition<BsonDocument> MatchByItemNames(
            ReceiptsFilters queryFilter)
        {
            if (!queryFilter.ItemNames.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var itemNames = queryFilter.ItemNames
                .Select(x => new BsonRegularExpression(new Regex(x, RegexOptions.IgnoreCase)));

            var filter = new BsonDocument(
                "ReceiptItems.ItemName",
                new BsonDocument("$in", BsonArray.Create(itemNames)));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
        }

        private static FilterDefinition<BsonDocument> MatchByReceiptIds(
            ReceiptsFilters queryFilter)
        {
            if (!queryFilter.ReceiptIds.Any())
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
