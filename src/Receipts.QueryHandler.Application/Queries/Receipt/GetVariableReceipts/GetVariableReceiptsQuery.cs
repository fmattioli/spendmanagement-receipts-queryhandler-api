using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Requests;
using Contracts.Web.Http.Receipt.Responses;
using MediatR;

namespace Receipts.QueryHandler.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<GetVariableReceiptResponse>>;
}
