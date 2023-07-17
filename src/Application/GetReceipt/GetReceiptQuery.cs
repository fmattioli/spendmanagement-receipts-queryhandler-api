using MediatR;
using Web.Contracts.UseCases.Common;

namespace Application.GetReceipt
{
    public record GetReceiptQuery(Guid ReceiptId) : IRequest<ReceiptResponse>;
}
