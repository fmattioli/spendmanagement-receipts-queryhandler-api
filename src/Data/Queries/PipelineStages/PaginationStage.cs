using Domain.Entities;
using Domain.Queries.GetReceipts;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Queries.PipelineStages
{
    internal static class PaginationStage
    {
        public static PipelineDefinition<Receipt, BsonDocument> Paginate(
            this PipelineDefinition<Receipt, BsonDocument> pipelineDefinition,
            ReceiptsFilters queryFilter)
        {
            var skipCount = (queryFilter.PageFilter.PageSize * (queryFilter.PageFilter.PageNumber - 1));
            var pageSizeLimit = queryFilter.PageFilter.PageSize;

            return pipelineDefinition
                .Skip(skipCount)
                .Limit(pageSizeLimit);
        }
    }
}
