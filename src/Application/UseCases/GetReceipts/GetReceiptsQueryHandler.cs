using MediatR;

namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsQueryHandler : IRequestHandler<GetReceiptsQuery, IReadOnlyCollection<GetReceiptsResponse>>
    {
        public Task<IReadOnlyCollection<GetReceiptsResponse>> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
