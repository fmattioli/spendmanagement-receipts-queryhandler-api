using Receipts.ReadModel.QueriesFilters;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Receipts.ReadModel.Data.Queries.PipelineStages.RecurringReceipt
{
    public static class FilterRecurringReceiptStage
    {
        public static PipelineDefinition<Entities.RecurringReceipt, BsonDocument> FilterRecurringReceipts(
            this PipelineDefinition<Entities.RecurringReceipt, BsonDocument> pipelineDefinition,
            RecurringReceiptFilters queryFilter)
        {
            var matchFilter = BuildMatchFilter(queryFilter);

            if (matchFilter != FilterDefinition<BsonDocument>.Empty)
            {
                pipelineDefinition = pipelineDefinition.Match(matchFilter);
            }

            return pipelineDefinition;
        }

        private static FilterDefinition<BsonDocument> BuildMatchFilter(RecurringReceiptFilters queryFilter)
        {
            var filters = new List<FilterDefinition<BsonDocument>>
            {
                MatchByRecurringReceiptIds(queryFilter),
                MatchByEstablishmentNames(queryFilter),
                MatchByRecurringReceiptCategoryIds(queryFilter)
            };

            filters.RemoveAll(x => x == FilterDefinition<BsonDocument>.Empty);

            if (filters.Count == 0)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            return filters.Count == 1 ? filters[0] : Builders<BsonDocument>.Filter.And(filters);
        }

        private static FilterDefinition<BsonDocument> MatchByRecurringReceiptIds(
            RecurringReceiptFilters queryFilter)
        {
            if (!(queryFilter?.ReceiptIds?.Any() ?? false))
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var receiptIds = queryFilter!.ReceiptIds!.Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "_id",
                new BsonDocument("$in", new BsonArray(receiptIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }

        private static FilterDefinition<BsonDocument> MatchByEstablishmentNames(
            RecurringReceiptFilters queryFilter)
        {
            if (!(queryFilter?.EstablishmentNames?.Any() ?? false))
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var establishmentNames = queryFilter!.EstablishmentNames!.Select(x => new BsonRegularExpression(new Regex(x, RegexOptions.IgnoreCase)));

            var filter = new BsonDocument(
                "EstablishmentName",
                new BsonDocument("$in", BsonArray.Create(establishmentNames)));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
        }

        private static FilterDefinition<BsonDocument> MatchByRecurringReceiptCategoryIds(
            RecurringReceiptFilters queryFilter)
        {
            if (!(queryFilter?.CategoryIds?.Any() ?? false))
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var categoryIds = queryFilter!.CategoryIds!.Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "Category._id",
                new BsonDocument("$in", new BsonArray(categoryIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }
    }
}
