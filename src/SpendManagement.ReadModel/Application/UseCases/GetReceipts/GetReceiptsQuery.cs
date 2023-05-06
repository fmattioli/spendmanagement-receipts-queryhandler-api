using MediatR;

namespace Application.UseCases.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsViewModel) : IRequest<IReadOnlyCollection<GetReceiptsResponse>>;
}
