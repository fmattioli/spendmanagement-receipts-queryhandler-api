using Application.Converters;
using Application.Extensions;

using Domain.Interfaces;
using MediatR;
using Web.Contracts.Receipt;

namespace Application.Queries.Receipt.GetReceipt
{
    public class GetReceiptQueryHandler : IRequestHandler<GetReceiptQuery, ReceiptResponse>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptQueryHandler(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public async Task<ReceiptResponse> Handle(GetReceiptQuery request, CancellationToken cancellationToken)
        {
            var receiptEntity = await _receiptRepository
                .FindOneAsync(x => x.Id == request.ReceiptId)
                .ValidateIfEntityIsValid();

            var response = receiptEntity.ToReceiptResponse();
            return response;
        }
    }
}
