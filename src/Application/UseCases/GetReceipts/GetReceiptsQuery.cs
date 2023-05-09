using MediatR;

namespace Application.UseCases.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsRequest) : IRequest<GetReceiptsResponse>;
}
