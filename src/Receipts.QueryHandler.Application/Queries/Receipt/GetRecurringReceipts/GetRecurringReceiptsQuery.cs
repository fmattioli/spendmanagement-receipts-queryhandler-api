using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Requests;
using Contracts.Web.Http.Receipt.Responses;
using MediatR;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetRecurringReceipts
{
    public record GetRecurringReceiptsQuery(GetRecurringReceiptsRequest GetRecurringReceiptsRequest) : IRequest<PagedResult<GetRecurringReceiptResponse>>;
}
