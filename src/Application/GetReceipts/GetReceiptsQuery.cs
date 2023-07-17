using MediatR;
using Web.Contracts.UseCases.GetReceipts;

namespace Application.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsRequest) : IRequest<GetReceiptsResponse>;
}
