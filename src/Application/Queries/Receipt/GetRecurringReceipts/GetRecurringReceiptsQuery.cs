using MediatR;
using SpendManagement.WebContracts.Common;
using Web.Contracts.Receipt;

namespace Application.Queries.Receipt.GetRecurringReceipts
{
    public record GetRecurringReceiptsQuery(GetRecurringReceiptsRequest GetRecurringReceiptsRequest) : IRequest<PagedResult<RecurringReceiptResponse>>;
}
