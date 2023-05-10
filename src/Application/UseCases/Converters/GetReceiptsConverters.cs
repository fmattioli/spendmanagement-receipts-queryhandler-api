using Application.UseCases.GetReceipts;
using Domain.Entities;
using Domain.Queries;
using Domain.Queries.GetReceipts;

namespace Application.UseCases.Converters
{
    public static class GetReceiptsConverters
    {
        public static GetReceiptsInput ToInput(this GetReceiptsRequest request)
        {
            return new GetReceiptsInput
            {
                EstablishmentNames = request.EstablishmentNames,
                ItemNames = request.ItemNames,
                ReceiptDate = request.ReceiptDate,
                ReceiptDateFinal = request.ReceiptDateFinal,
                ReceiptIds = request.ReceiptIds,
                ReceiptItemIds = request.ReceiptItemIds,
                PageFilter = new Common.PageFilter
                {
                    PageNumber = request.PageFilter.Page,
                    PageSize = request.PageFilter.PageSize
                }
            };
        }

        public static ReceiptsFilters ToDomainFilters(this GetReceiptsInput getReceiptsInput)
        {
            return new ReceiptsFilters(
                getReceiptsInput.ReceiptIds,
                getReceiptsInput.ReceiptItemIds,
                getReceiptsInput.EstablishmentNames,
                getReceiptsInput.ItemNames,
                getReceiptsInput.ReceiptDate,
                getReceiptsInput.ReceiptDateFinal,
                getReceiptsInput.PageFilter.PageNumber,
                getReceiptsInput.PageFilter.PageSize);
        }

        public static GetReceiptsResponse ToResponse(this PagedResultFilter<Receipt> receipts)
        {
            return new GetReceiptsResponse
            {
                PageNumber = receipts.PageNumber,
                PageSize = receipts.PageSize,
                PageSizeLimit = receipts.PageSizeLimit,
                Results = receipts.Results,
                TotalPages = receipts.TotalPages,
                TotalResults = receipts.TotalResults
            };
        }
    }
}
