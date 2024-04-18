using Contracts.Web.Common;
using Contracts.Web.Receipt.Requests;
using Contracts.Web.Receipt.Responses;
using MediatR;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetRecurringReceipts
{
    public record GetRecurringReceiptsQuery(GetRecurringReceiptsRequest GetRecurringReceiptsRequest) : IRequest<PagedResult<RecurringReceiptResponse>>;
}
