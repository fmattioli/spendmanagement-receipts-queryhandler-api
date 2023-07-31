using MediatR;
using Web.Contracts.Receipt;

namespace Application.Queries.Receipt.GetReceipt
{
    public record GetReceiptQuery(Guid ReceiptId) : IRequest<ReceiptResponse>;
}
