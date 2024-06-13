using Contracts.Web.Common;
using Contracts.Web.Receipt.Responses;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Application.Providers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetRecurringReceipts
{
    public class GetRecurringReceiptsQueryHandler(IReceiptRepository receiptRepository, IAuthProvider authProvider) : IRequestHandler<GetRecurringReceiptsQuery, PagedResult<GetRecurringReceiptResponse>>
    {
        private readonly IReceiptRepository recurringReceiptRepository = receiptRepository;
        private readonly IAuthProvider _authProvider = authProvider;

        public async Task<PagedResult<GetRecurringReceiptResponse>> Handle(GetRecurringReceiptsQuery request, CancellationToken cancellationToken)
        {
            var tenantId = _authProvider.GetTenant();
            var domainFilters = request.GetRecurringReceiptsRequest.ToDomainFilters(tenantId);
            var recurringReceiptQueryResult = await recurringReceiptRepository.GetRecurringReceiptsAsync(domainFilters);
            return recurringReceiptQueryResult.ToResponse(request.GetRecurringReceiptsRequest.PageFilter);
        }
    }
}
