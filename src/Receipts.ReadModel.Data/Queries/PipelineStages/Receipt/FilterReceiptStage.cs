using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using Receipts.ReadModel.QueriesFilters;

namespace Receipts.ReadModel.Data.Queries.PipelineStages.Receipt
{
    public static class FilterReceiptStage
    {
        public static PipelineDefinition<Entities.Receipt, BsonDocument> FilterReceipts(
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

        private static FilterDefinition<BsonDocument> MatchByReceiptIds(
            ReceiptFilters queryFilter)
        {
            if (!queryFilter.ReceiptIds.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var receiptIds = queryFilter.ReceiptIds
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "_id",
                new BsonDocument("$in", new BsonArray(receiptIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }

        private static FilterDefinition<BsonDocument> MatchByCategoryIds(
            ReceiptFilters queryFilter)
        {
            if (!queryFilter.CategoryIds.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var categoryIds = queryFilter.CategoryIds
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var receiptFilter = new BsonDocument(
                "CategoryId",
                new BsonDocument("$in", new BsonArray(categoryIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(receiptFilter);
        }

        private static FilterDefinition<BsonDocument> MatchByReceiptDate(
            ReceiptFilters queryFilter)
        {
            if (queryFilter.ReceiptDate == DateTime.MinValue && queryFilter.ReceiptDateFinal == DateTime.MinValue)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var query = new BsonDocument("ReceiptDate",
                new BsonDocument
                {
                    { "$gte", new DateTime(queryFilter.ReceiptDate.Year, queryFilter.ReceiptDate.Month, queryFilter.ReceiptDate.Day, 0, 0, 0, DateTimeKind.Utc) },
                    { "$lte", new DateTime(queryFilter.ReceiptDateFinal.Year, queryFilter.ReceiptDateFinal.Month, queryFilter.ReceiptDateFinal.Day, 23, 59, 59, DateTimeKind.Utc) },
                });


            return new BsonDocumentFilterDefinition<BsonDocument>(query);
        }

        private static FilterDefinition<BsonDocument> MatchByEstablishmentNames(
            ReceiptFilters queryFilter)
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
