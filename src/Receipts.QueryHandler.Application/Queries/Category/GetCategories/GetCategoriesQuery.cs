using Contracts.Web.Category.Requests;
using MediatR;

namespace Receipts.QueryHandler.Application.Queries.Category.GetCategories
{
    public record GetCategoriesQuery(GetCategoriesRequest GetReceiptsRequest) : IRequest<GetCategoriesResponse>;
}

