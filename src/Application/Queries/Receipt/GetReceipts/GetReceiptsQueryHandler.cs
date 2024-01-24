using Application.Converters;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Receipt.GetReceipts
{
    public class GetReceiptsQueryHandler(IReceiptRepository receiptRepository) : IRequestHandler<GetReceiptsQuery, GetReceiptsResponse>
    {
        private readonly IReceiptRepository receiptRepository = receiptRepository;

        public async Task<GetReceiptsResponse> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetReceiptsRequest.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse();
        }
    }
}
