using MediatR;
using Web.Contracts.UseCases.GetReceipts;

namespace Application.Queries.Receipt.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsRequest) : IRequest<GetReceiptsResponse>;
}
