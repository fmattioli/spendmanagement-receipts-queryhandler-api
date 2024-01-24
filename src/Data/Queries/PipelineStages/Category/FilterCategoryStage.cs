using Domain.QueriesFilters;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace Data.Queries.PipelineStages.Category
{
    internal static class FilterCategoryStage
    {
        internal static PipelineDefinition<Domain.Entities.Category, BsonDocument> FilterCategories(
            this PipelineDefinition<Domain.Entities.Category, BsonDocument> pipelineDefinition,
            CategoryFilters queryFilter)
        {
            var matchFilter = BuildMatchFilter(queryFilter);

            if (matchFilter != FilterDefinition<BsonDocument>.Empty)
            {
                pipelineDefinition = pipelineDefinition.Match(matchFilter);
            }

            return pipelineDefinition;
        }

        private static FilterDefinition<BsonDocument> BuildMatchFilter(CategoryFilters queryFilter)
        {
            var filters = new List<FilterDefinition<BsonDocument>>
            {
                MatchByCategoriesIds(queryFilter),
                MatchByCategoryNames(queryFilter),
            };

            filters.RemoveAll(x => x == FilterDefinition<BsonDocument>.Empty);

            if (filters.Count == 0)
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            return filters.Count == 1 ? filters[0] : Builders<BsonDocument>.Filter.And(filters);
        }

        private static FilterDefinition<BsonDocument> MatchByCategoriesIds(
            CategoryFilters queryFilter)
        {
            if (!queryFilter.CategoryIds.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var categoriesIds = queryFilter.CategoryIds
                .Select(x => new BsonBinaryData(x, GuidRepresentation.Standard));

            var categories = new BsonDocument(
                "_id",
                new BsonDocument("$in", new BsonArray(categoriesIds)));

            return new BsonDocumentFilterDefinition<BsonDocument>(categories);
        }

        private static FilterDefinition<BsonDocument> MatchByCategoryNames(
            CategoryFilters queryFilter)
        {
            if (!queryFilter.CategoryNames.Any())
            {
                return FilterDefinition<BsonDocument>.Empty;
            }

            var categoryNames = queryFilter.CategoryNames
                .Select(categoryName => new BsonRegularExpression(new Regex(categoryName.Trim(), RegexOptions.IgnoreCase)));

            var filter = new BsonDocument(
                "Name",
                new BsonDocument("$in", BsonArray.Create(categoryNames)));

            return new BsonDocumentFilterDefinition<BsonDocument>(filter);
        }
    }
}
