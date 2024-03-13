using MediatR;
using SpendManagement.WebContracts.Common;
using Web.Contracts.Receipt;

namespace Application.Queries.Receipt.GetReceipts
{
    public record GetReceiptsQuery(GetReceiptsRequest GetReceiptsRequest) : IRequest<PagedResult<ReceiptResponse>>;
}
