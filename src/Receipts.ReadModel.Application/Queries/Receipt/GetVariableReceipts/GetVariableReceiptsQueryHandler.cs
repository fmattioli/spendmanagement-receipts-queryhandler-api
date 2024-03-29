using Application.Converters;
using Receipts.ReadModel.Interfaces;
using MediatR;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Receipt;

namespace Application.Queries.Receipt.GetVariableReceipts
{
    public class GetVariableReceiptsQueryHandler(IReceiptRepository receiptRepository) : IRequestHandler<GetVariableReceiptsQuery, PagedResult<ReceiptResponse>>
    {
        private readonly IReceiptRepository receiptRepository = receiptRepository;

        public async Task<PagedResult<ReceiptResponse>> Handle(GetVariableReceiptsQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetVariableReceiptsRequest.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetVariableReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetVariableReceiptsRequest.PageFilter);
        }
    }
}
