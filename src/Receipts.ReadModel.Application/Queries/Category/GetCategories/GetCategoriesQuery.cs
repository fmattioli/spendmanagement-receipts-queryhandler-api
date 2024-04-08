using MediatR;
using Web.Contracts.Category.Requests;

namespace Receipts.ReadModel.Application.Queries.Category.GetCategories
{
    public record GetCategoriesQuery(GetCategoriesRequest GetReceiptsRequest) : IRequest<GetCategoriesResponse>;
}

