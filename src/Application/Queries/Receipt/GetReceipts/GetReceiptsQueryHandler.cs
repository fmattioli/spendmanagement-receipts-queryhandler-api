using Application.Converters;
using Domain.Interfaces;
using MediatR;

namespace Application.Queries.Receipt.GetReceipts
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
            var getReceiptsInput = request.GetReceiptsRequest.ToInput();
            var domainFilters = getReceiptsInput.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse();
        }
    }
}
