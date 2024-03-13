using Application.Converters;
using Domain.Interfaces;
using MediatR;
using SpendManagement.WebContracts.Common;
using Web.Contracts.Receipt;

namespace Application.Queries.Receipt.GetReceipts
{
    public class GetReceiptsQueryHandler(IReceiptRepository receiptRepository) : IRequestHandler<GetReceiptsQuery, PagedResult<ReceiptResponse>>
    {
        private readonly IReceiptRepository receiptRepository = receiptRepository;

        public async Task<PagedResult<ReceiptResponse>> Handle(GetReceiptsQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetReceiptsRequest.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetReceiptsRequest.PageFilter);
        }
    }
}
