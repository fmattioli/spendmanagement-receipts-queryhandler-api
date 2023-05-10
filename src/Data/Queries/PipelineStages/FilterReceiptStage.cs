using Domain.Entities;
using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;

using System.Text.RegularExpressions;

namespace Data.Queries.PipelineStages
{
    internal static class FilterReceiptStage
    {
        internal static PipelineDefinition<Receipt, BsonDocument> FilterReceipts(
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
                MatchByReceiptIds(queryFilter),
                MatchByEstablishmentNames(queryFilter)
            };

            filters.RemoveAll(x => x == FilterDefinition<BsonDocument>.Empty);

            if (!filters.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            return filters.Count == 1 ? filters.First() : Builders<BsonDocument>.Filter.And(filters);
        }

        private static FilterDefinition<BsonDocument> MatchByReceiptIds(
            ReceiptsFilters queryFilter)
        {
            if (!queryFilter.ReceiptIds.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var receiptItemIds = queryFilter.ReceiptIds
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptIds = new BsonDocument(
                "_id",
                new BsonDocument("$inmn", new BsonArray(receiptItemIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptIds);
        }
        
        private static FilterDefinition<BsonDocument> MatchByEstablishmentNames(
            ReceiptsFilters queryFilter)
        {
            if (!queryFilter.EstablishmentNames.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var establishmentNames = queryFilter.EstablishmentNames
                .Select(x => new BsonRegularExpression(new Regex(x, RegexOptions.IgnoreCase)));

            var filter = new BsonDocument(
                "EstablishmentName",
                new BsonDocument("$in", BsonArray.Create(establishmentNames)));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
        }
    }
}
