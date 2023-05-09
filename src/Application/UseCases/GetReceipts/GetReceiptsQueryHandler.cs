using Domain.Interfaces;
using MediatR;

namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsQueryHandler : IRequestHandler<GetReceiptsQuery, GetReceiptsResponse>
    {
        private readonly IReceiptRepository receiptRepository;

        public GetReceiptsQueryHandler(IReceiptRepository receiptRepository)
        {
            this.receiptRepository = receiptRepository;
        }

        public async Task<GetReceiptsResponse> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
        {
            var filters = request.GetReceiptsRequest.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetReceiptsAsync(filters);
            return receiptQueryResult.ToResponse();
        }
    }
}
