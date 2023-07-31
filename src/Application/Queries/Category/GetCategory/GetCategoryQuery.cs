using MediatR;
using Web.Contracts.Category;

namespace Application.Queries.Category.GetCategory
{
    public record GetCategoryQuery(Guid ReceiptId) : IRequest<CategoryResponse>;
}
