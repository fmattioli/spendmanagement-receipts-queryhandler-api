using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Data.Queries.PipelineStages.Receipt
{
    internal static class FilterReceiptStage
    {
        internal static PipelineDefinition<Domain.Entities.Receipt, BsonDocument> FilterReceipts(
            this PipelineDefinition<Domain.Entities.Receipt, BsonDocument> pipelineDefinition,
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

        private static FilterDefinition<BsonDocument> MatchByReceiptDate(
            ReceiptFilters queryFilter)
        {
            if (queryFilter.ReceiptDate == DateTime.MinValue && queryFilter.ReceiptDateFinal == DateTime.MinValue)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var query = queryFilter.ReceiptDate switch
            {
                _ when queryFilter.ReceiptDate != DateTime.MinValue && queryFilter.ReceiptDateFinal != DateTime.MinValue =>
                    new BsonDocument("ReceiptDate",
                    new BsonDocument
                    {
                        { "$gte", queryFilter.ReceiptDate },
                        { "$lt", queryFilter.ReceiptDateFinal!.Value.AddHours(23).AddMinutes(59) },
                    }),
                _ when queryFilter.ReceiptDate != DateTime.MinValue && queryFilter.ReceiptDateFinal == DateTime.MinValue =>
                    new BsonDocument("ReceiptDate",
                    new BsonDocument
                    {
                        { "$gte", queryFilter.ReceiptDate },
                    }),
                _ when queryFilter.ReceiptDateFinal != DateTime.MinValue && queryFilter.ReceiptDate == DateTime.MinValue =>
                     new BsonDocument("ReceiptDate",
                     new BsonDocument
                     {
                        { "$lte", queryFilter.ReceiptDateFinal!.Value.AddHours(23).AddMinutes(59) },
                     }),
                _ => throw new Exception("Invalid filter provided"),
            };

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
    }
}
