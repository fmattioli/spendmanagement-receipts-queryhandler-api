using Contracts.Web.Common;
using Contracts.Web.Receipt.Responses;
using MediatR;
using Receipts.ReadModel.Application.Mappers;
using Receipts.ReadModel.Interfaces;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetRecurringReceipts
{
    public class GetRecurringReceiptsQueryHandler(IReceiptRepository receiptRepository) : IRequestHandler<GetRecurringReceiptsQuery, PagedResult<RecurringReceiptResponse>>
    {
        private readonly IReceiptRepository recurringReceiptRepository = receiptRepository;

        public async Task<PagedResult<RecurringReceiptResponse>> Handle(GetRecurringReceiptsQuery request, CancellationToken cancellationToken)
        {
            var domainFilters = request.GetRecurringReceiptsRequest.ToDomainFilters();
            var recurringReceiptQueryResult = await recurringReceiptRepository.GetRecurringReceiptsAsync(domainFilters);
            return recurringReceiptQueryResult.ToResponse(request.GetRecurringReceiptsRequest.PageFilter);
        }
    }
}
