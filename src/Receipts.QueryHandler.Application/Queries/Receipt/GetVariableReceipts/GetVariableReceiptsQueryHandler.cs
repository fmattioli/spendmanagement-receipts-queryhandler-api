using Contracts.Web.Common;
using Contracts.Web.Receipt.Responses;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Application.Providers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts
{
    public class GetVariableReceiptsQueryHandler(IReceiptRepository receiptRepository, IAuthProvider authProvider) : IRequestHandler<GetVariableReceiptsQuery, PagedResult<GetVariableReceiptResponse>>
    {
        private readonly IReceiptRepository _receiptRepository = receiptRepository;
        private readonly IAuthProvider _authProvider = authProvider;

        public async Task<PagedResult<GetVariableReceiptResponse>> Handle(GetVariableReceiptsQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _authProvider.GetTenant();
            var domainFilters = request.GetVariableReceiptsRequest.ToDomainFilters(tenantId);
            var receiptQueryResult = await _receiptRepository.GetVariableReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetVariableReceiptsRequest.PageFilter);
        }
    }
}
