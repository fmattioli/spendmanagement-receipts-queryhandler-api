using Domain.Entities;
using Domain.Queries;
using Domain.Queries.GetReceipts;

namespace Application.UseCases.GetReceipts
{
    public static class GetReceiptsRequestConverter
    {
        public static GetReceiptsFilter ToDomainFilters(this GetReceiptsRequest getReceiptsRequest)
        {
            return new GetReceiptsFilter(
                getReceiptsRequest.ReceiptIds,
                getReceiptsRequest.ReceiptItemIds,
                getReceiptsRequest.EstablishmentNames,
                getReceiptsRequest.ReceiptDate,
                getReceiptsRequest.ReceiptDateFinal);
        }

        public static GetReceiptsResponse ToResponse(this PagedResultFilter<Receipt> receipts)
        {
            return new GetReceiptsResponse
            {
                PageNumber = receipts.PageNumber,
                PageSize = receipts.PageSize,
                PageSizeLimit = receipts.PageSizeLimit,
                Results= receipts.Results,
                TotalPages = receipts.TotalPages, 
                TotalResults = receipts.TotalResults
            };
        }
    }
}
