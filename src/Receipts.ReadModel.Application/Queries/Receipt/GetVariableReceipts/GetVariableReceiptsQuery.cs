using MediatR;
using Web.Contracts.Common;
using Web.Contracts.Receipt;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
