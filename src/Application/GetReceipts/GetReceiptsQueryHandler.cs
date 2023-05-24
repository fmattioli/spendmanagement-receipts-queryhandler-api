using Application.Converters;
using Domain.Interfaces;
using MediatR;
using Web.Contracts.UseCases.GetReceipts;

namespace Application.GetReceipts
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
