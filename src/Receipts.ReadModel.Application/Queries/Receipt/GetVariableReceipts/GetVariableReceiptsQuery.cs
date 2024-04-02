using MediatR;
using Receipts.WebContracts.Common;
using Receipts.WebContracts.Receipt;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
