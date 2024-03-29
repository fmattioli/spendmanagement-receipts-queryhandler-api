using MediatR;

namespace Application.Queries.Category.GetCategories
{
    public record GetCategoriesQuery(GetCategoriesRequest GetReceiptsRequest) : IRequest<GetCategoriesResponse>;
}

