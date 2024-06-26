﻿using MongoDB.Bson;
using MongoDB.Driver;

using Receipts.QueryHandler.Data.Constants;
using Receipts.QueryHandler.Domain.QueriesFilters;
using System.Text.RegularExpressions;

namespace Receipts.QueryHandler.Data.Queries.PipelineStages.Receipt
{
    public static class FilterReceiptStage
    {
        public static PipelineDefinition<Domain.Entities.VariableReceipt, BsonDocument> FilterReceipts(
            this PipelineDefinition<Domain.Entities.VariableReceipt, BsonDocument> pipelineDefinition,
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
                MatchByUser(queryFilter.TenantId, queryFilter.UserId),
                MatchByTenant(queryFilter.TenantId),
                MatchByReceiptIds(queryFilter),
                MatchByCategoryIds(queryFilter),
                MatchByEstablishmentNames(queryFilter),
                MatchByReceiptDate(queryFilter)
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

        private static FilterDefinition<BsonDocument> MatchByReceiptIds(
            ReceiptFilters queryFilter)
        {
            if (!(queryFilter?.ReceiptIds?.Any() ?? false))
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var receiptIds = queryFilter!.ReceiptIds!
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "_id",
                new BsonDocument("$in", new BsonArray(receiptIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }

        private static FilterDefinition<BsonDocument> MatchByCategoryIds(
            ReceiptFilters queryFilter)
        {
            if (!(queryFilter?.CategoryIds?.Any() ?? false))
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var categoryIds = queryFilter!.CategoryIds!
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "Category._id",
                new BsonDocument("$in", new BsonArray(categoryIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }

        private static FilterDefinition<BsonDocument> MatchByReceiptDate(
            ReceiptFilters queryFilter)
        {
            if (queryFilter.ReceiptDateInitial == DateTime.MinValue && queryFilter.ReceiptDateEnd == DateTime.MinValue)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var query = new BsonDocument("ReceiptDate",
                new BsonDocument
                {
                    { "$gte", new DateTime(queryFilter.ReceiptDateInitial.Year, queryFilter.ReceiptDateInitial.Month, queryFilter.ReceiptDateInitial.Day, 0, 0, 0, DateTimeKind.Utc) },
                    { "$lte", new DateTime(queryFilter.ReceiptDateEnd.Year, queryFilter.ReceiptDateEnd.Month, queryFilter.ReceiptDateEnd.Day, 23, 59, 59, DateTimeKind.Utc) },
                });


            return new BsonDocumentFilterDefinition<BsonDocument>(query);
        }

        private static FilterDefinition<BsonDocument> MatchByEstablishmentNames(
            ReceiptFilters queryFilter)
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

        public static PipelineDefinition<T, BsonDocument> MakeSumBasedOnFilterName<T>(
            this PipelineDefinition<T, BsonDocument> pipelineDefinition, string totalFieldName)
        {
            return pipelineDefinition
                .AppendStage(AddTotalField(totalFieldName))
                .AppendStage(SumTotalField(totalFieldName));
        }

        private static PipelineStageDefinition<BsonDocument, BsonDocument> AddTotalField(string totalFieldName)
        {
            return new BsonDocument("$addFields", new BsonDocument
            {
                { "TotalDecimal", new BsonDocument("$toDecimal", $"${totalFieldName}") }
            });
        }

        private static PipelineStageDefinition<BsonDocument, BsonDocument> SumTotalField(string totalFieldName)
        {
            return new BsonDocument("$group", new BsonDocument
            {
                { "_id", "1" },
                { $"{totalFieldName}", new BsonDocument("$sum", "$TotalDecimal") }
            });
        }
    }
}
