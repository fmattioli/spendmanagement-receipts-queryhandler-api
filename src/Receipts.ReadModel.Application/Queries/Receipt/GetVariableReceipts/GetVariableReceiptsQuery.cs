using MediatR;
using Web.Contracts.Common;
using Web.Contracts.Receipt.Requests;
using Web.Contracts.Receipt.Responses;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
