using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Data.Constants;

namespace Receipts.QueryHandler.Data.Queries.PipelineStages.RecurringReceipt
{
    public static class FilterRecurringReceiptStage
    {
        public static PipelineDefinition<Domain.Entities.RecurringReceipt, BsonDocument> FilterRecurringReceipts(
            this PipelineDefinition<Domain.Entities.RecurringReceipt, BsonDocument> pipelineDefinition,
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
                MatchByUser(queryFilter.TenantId, queryFilter.UserId),
                MatchByTenant(queryFilter.TenantId),
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

        private static FilterDefinition<BsonDocument> MatchByUser(int tenantId, Guid userId)
        {
            if (tenantId == DataConstants.DefaultTenant)
            {

                var filter = new BsonDocument(
                    "UserId",
                    new BsonDocument("$eq", new BsonBinaryData(userId, GuidRepresentation.Standard)));

                return new BsonDocumentFilterDefinition<BsonDocument>(filter);
            }

            return FilterDefinition<BsonDocument>.Empty;
        }

        private static FilterDefinition<BsonDocument> MatchByTenant(int tenantId)
        {
            var filter = new BsonDocument(
                "Tenant.Number",
                new BsonDocument("$eq", tenantId));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
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
