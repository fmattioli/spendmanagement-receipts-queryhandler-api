using Application.Queries.Receipt.GetReceipts;
using Domain.Entities;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters.PageFilters;
using Domain.ValueObjects;
using Web.Contracts.Receipt;

namespace Application.Converters
{
    public static class ReceiptMapper
    {
        public static ReceiptFilters ToDomainFilters(this GetReceiptsRequest getReceiptsInput)
        {
            return new ReceiptFilters(
                getReceiptsInput.ReceiptIds,
                getReceiptsInput.ReceiptItemIds,
                getReceiptsInput.EstablishmentNames,
                getReceiptsInput.ItemNames,
                getReceiptsInput.ReceiptDate,
                getReceiptsInput.ReceiptDateFinal,
                getReceiptsInput.PageFilter.Page,
                getReceiptsInput.PageFilter.PageSize);
        }

        public static GetReceiptsResponse ToResponse(this PagedResultFilter<Receipt> receipts)
        {
            return new GetReceiptsResponse
            {
                PageNumber = receipts.PageNumber,
                PageSize = receipts.PageSize,
                PageSizeLimit = receipts.PageSizeLimit,
                Results = receipts.Results.SelectMany(x => x.ToResponseItems()),
                TotalPages = receipts.TotalPages,
                TotalResults = receipts.TotalResults
            };
        }

        public static IEnumerable<ReceiptResponse> ToResponseItems(this Receipt receipt)
        {
            return new List<ReceiptResponse>
            {
                new ReceiptResponse
                {
                    EstablishmentName = receipt.EstablishmentName,
                    Id = receipt.Id,
                    ReceiptDate = receipt.ReceiptDate,
                    ReceiptItems = receipt.ReceiptItems.SelectMany(x => x.ToDomainReceiptItems())
                }
            };
        }

        public static IEnumerable<ReceiptItemResponse> ToDomainReceiptItems(this ReceiptItem receiptItem)
        {
            return new List<ReceiptItemResponse>()
            {
                new ReceiptItemResponse
                {
                    Id = receiptItem.Id,
                    ItemName = receiptItem.ItemName,
                    ItemPrice = receiptItem.ItemPrice,
                    Observation = receiptItem.Observation,
                    Quantity = receiptItem.Quantity,
                    CategoryId = receiptItem.CategoryId,
                }
            };
        }

        public static ReceiptResponse ToReceiptResponse(this Receipt receipt)
        {
            return new ReceiptResponse
            {
               Id = receipt.Id,
               EstablishmentName = receipt.EstablishmentName,
               ReceiptDate = receipt.ReceiptDate,
               ReceiptItems = receipt.ReceiptItems.SelectMany(x => x.ToDomainReceiptItems())
            };
        }
    }
}
