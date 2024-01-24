using MediatR;

namespace Application.Queries.Receipt.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsRequest) : IRequest<GetReceiptsResponse>;
}
