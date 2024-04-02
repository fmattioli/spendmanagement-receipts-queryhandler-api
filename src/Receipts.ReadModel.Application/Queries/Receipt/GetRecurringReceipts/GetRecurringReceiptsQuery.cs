using MediatR;
using Receipts.WebContracts.Common;
using Receipts.WebContracts.Receipt;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetRecurringReceipts
{
    public record GetRecurringReceiptsQuery(GetRecurringReceiptsRequest GetRecurringReceiptsRequest) : IRequest<PagedResult<RecurringReceiptResponse>>;
}
