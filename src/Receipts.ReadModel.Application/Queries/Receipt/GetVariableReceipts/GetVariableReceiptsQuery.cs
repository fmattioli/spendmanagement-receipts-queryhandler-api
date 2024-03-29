using MediatR;
using SpendManagement.WebContracts.Common;
using SpendManagement.WebContracts.Receipt;

namespace Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsQuery(GetVariableReceiptsRequest GetVariableReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
