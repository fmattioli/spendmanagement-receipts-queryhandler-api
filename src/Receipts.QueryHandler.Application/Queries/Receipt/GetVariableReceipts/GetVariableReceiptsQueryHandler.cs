using Contracts.Web.Common;
using Contracts.Web.Receipt.Responses;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts
{
    public class GetVariableReceiptsQueryHandler(IReceiptRepository receiptRepository) : IRequestHandler<GetVariableReceiptsQuery, PagedResult<GetVariableReceiptResponse>>
    {
        private readonly IReceiptRepository receiptRepository = receiptRepository;

        public async Task<PagedResult<GetVariableReceiptResponse>> Handle(GetVariableReceiptsQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetVariableReceiptsRequest.ToDomainFilters();
            var receiptQueryResult = await receiptRepository.GetVariableReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetVariableReceiptsRequest.PageFilter);
        }
    }
}
