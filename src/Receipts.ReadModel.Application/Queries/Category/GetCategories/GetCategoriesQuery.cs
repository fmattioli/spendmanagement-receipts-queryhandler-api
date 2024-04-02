using MediatR;

namespace Receipts.ReadModel.Application.Queries.Category.GetCategories
{
    public record GetCategoriesQuery(GetCategoriesRequest GetReceiptsRequest) : IRequest<GetCategoriesResponse>;
}

