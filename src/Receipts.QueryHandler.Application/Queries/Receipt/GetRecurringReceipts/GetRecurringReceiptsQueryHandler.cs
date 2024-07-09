using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Responses;
using Feijuca.Keycloak.MultiTenancy.Services;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetRecurringReceipts
{
    public class GetRecurringReceiptsQueryHandler(IReceiptRepository receiptRepository, IAuthService authService) : IRequestHandler<GetRecurringReceiptsQuery, PagedResult<GetRecurringReceiptResponse>>
    {
        private readonly IReceiptRepository recurringReceiptRepository = receiptRepository;
        private readonly IAuthService _authService = authService;

        public async Task<PagedResult<GetRecurringReceiptResponse>> Handle(GetRecurringReceiptsQuery request, CancellationToken cancellationToken)
        {
            var tenantId = int.Parse(_authService.GetTenantFromToken());
            Guid userId = _authService.GetUserFromToken();

            var domainFilters = request.GetRecurringReceiptsRequest.ToDomainFilters(userId, tenantId);
            var recurringReceiptQueryResult = await recurringReceiptRepository.GetRecurringReceiptsAsync(domainFilters);
            return recurringReceiptQueryResult.ToResponse(request.GetRecurringReceiptsRequest.PageFilter);
        }
    }
}
