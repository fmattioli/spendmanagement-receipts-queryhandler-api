using MediatR;
using Web.Contracts.Common;
using Web.Contracts.Receipt;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetRecurringReceipts
{
    public record GetRecurringReceiptsQuery(GetRecurringReceiptsRequest GetRecurringReceiptsRequest) : IRequest<PagedResult<RecurringReceiptResponse>>;
}
