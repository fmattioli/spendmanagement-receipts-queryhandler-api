using Application.Converters;
using Domain.Interfaces;
using MediatR;
using Web.Contracts.UseCases.Common;

namespace Application.GetReceipt
{
    public class GetReceiptQueryHandler : IRequestHandler<GetReceiptQuery, ReceiptResponse>
    {
        private readonly IReceiptRepository _receiptRepository;

        public GetReceiptQueryHandler(IReceiptRepository receiptRepository)
        {
            this._receiptRepository = receiptRepository;
        }

        public async Task<ReceiptResponse> Handle(GetReceiptQuery request, CancellationToken cancellationToken)
        {
            var receiptEntity = await _receiptRepository.FindOneAsync(x => x.Id == request.ReceiptId);
            var response = receiptEntity.ToReceiptResponse();
            return response;
        }
    }
}
