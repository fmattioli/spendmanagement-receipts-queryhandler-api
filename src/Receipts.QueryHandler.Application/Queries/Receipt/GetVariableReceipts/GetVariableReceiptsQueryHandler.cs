using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Responses;
using Contracts.Web.Models;
using Contracts.Web.Services.Auth;
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
            int tenantId = _authService.GetTenant();
            Guid userId = _authService.GetUser();

            var authModel = new AuthModel(tenantId, userId);

            var domainFilters = request.GetVariableReceiptsRequest.ToDomainFilters(authModel);
            var receiptQueryResult = await _receiptRepository.GetVariableReceiptsAsync(domainFilters);
            return receiptQueryResult.ToResponse(request.GetVariableReceiptsRequest.PageFilter);
        }
    }
}
