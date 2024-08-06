using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Responses;
using Feijuca.Keycloak.MultiTenancy.Services;
using MediatR;
using Receipts.QueryHandler.Application.Mappers;
using Receipts.QueryHandler.Domain.Interfaces;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts
{
    public class GetVariableReceiptsQueryHandler(IReceiptRepository receiptRepository, IAuthService authService) : IRequestHandler<GetVariableReceiptsQuery, PagedResult<GetVariableReceiptResponse>>
    {
        private readonly IReceiptRepository _receiptRepository = receiptRepository;
        private readonly IAuthService _authService = authService;

        public async Task<PagedResult<GetVariableReceiptResponse>> Handle(GetVariableReceiptsQuery request, CancellationToken cancellationToken)
        {
            var tenantId = int.Parse(_authService.GetTenantFromToken());
            Guid userId = _authService.GetUserIdFromToken();

            var domainFilters = request.GetVariableReceiptsRequest.ToDomainFilters(userId, tenantId);
            var receiptQueryResult = await _receiptRepository.GetVariableReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetVariableReceiptsRequest.PageFilter);
        }
    }
}
