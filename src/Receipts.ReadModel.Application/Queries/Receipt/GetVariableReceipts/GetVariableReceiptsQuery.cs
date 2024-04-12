using Contracts.Web.Common;
using Contracts.Web.Receipt.Requests;
using Contracts.Web.Receipt.Responses;
using MediatR;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
