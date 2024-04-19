using Contracts.Web.Common;
using Contracts.Web.Receipt.Requests;
using Contracts.Web.Receipt.Responses;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;
using Receipts.QueryHandler.Domain.ValueObjects;

namespace Receipts.QueryHandler.Application.Mappers
{
    public static class ReceiptMapper
    {
        public static ReceiptFilters ToDomainFilters(this GetVariableReceiptsRequest getVariableReceiptsInput)
        {
            return new ReceiptFilters(
                getVariableReceiptsInput.ReceiptIds,
                getVariableReceiptsInput.CategoryIds,
                getVariableReceiptsInput.ReceiptItemIds,
                getVariableReceiptsInput.EstablishmentNames,
                getVariableReceiptsInput.ReceiptItemNames,
                getVariableReceiptsInput.ReceiptDate,
                getVariableReceiptsInput.ReceiptDateFinal,
                getVariableReceiptsInput.PageFilter.Page,
                getVariableReceiptsInput.PageFilter.PageSize);
        }

        public static RecurringReceiptFilters ToDomainFilters(this GetRecurringReceiptsRequest getReceiptsInput)
        {
            return new RecurringReceiptFilters(
                getReceiptsInput.ReceiptIds,
                getReceiptsInput.CategoryIds,
                getReceiptsInput.EstablishmentNames,
                getReceiptsInput.PageFilter.Page,
                getReceiptsInput.PageFilter.PageSize);
        }

        public static PagedResult<ReceiptResponse> ToResponse(this PagedResultFilter<Receipt> receipts, PageFilterRequest pageFilter)
        {
            return new PagedResult<ReceiptResponse>
            {
                PageNumber = pageFilter.Page,
                PageSize = pageFilter.PageSize,
                Results = receipts.Results.SelectMany(x => x.ToReceiptResponseItems()),
                TotalAmount = receipts.ReceiptsTotalAmount,
                TotalPages = receipts.TotalPages,
                TotalResults = receipts.TotalResults
            };
        }

        public static PagedResult<RecurringReceiptResponse> ToResponse(this PagedResultFilter<RecurringReceipt> recurringReceipts, PageFilterRequest pageFilter)
        {
            return new PagedResult<RecurringReceiptResponse>
            {
                PageNumber = pageFilter.Page,
                PageSize = pageFilter.PageSize,
                Results = recurringReceipts.Results.SelectMany(x => x.ToRecurringReceiptResponseItems()),
                TotalPages = recurringReceipts.TotalPages,
                TotalResults = recurringReceipts.TotalResults,
                TotalAmount = recurringReceipts.ReceiptsTotalAmount
            };
        }

        public static ReceiptResponse ToReceiptResponse(this Receipt receipt)
        {
            return new ReceiptResponse
            {
                Id = receipt.Id,
                Category = receipt.Category.ToCategoryResponse(),
                EstablishmentName = receipt.EstablishmentName,
                ReceiptDate = receipt.ReceiptDate,
                ReceiptItems = receipt.ReceiptItems.SelectMany(x => x.ToDomainReceiptItems()),
                Discount = receipt.Discount,
                Total = receipt.Total,
            };
        }

        public static IEnumerable<ReceiptItemResponse> ToDomainReceiptItems(this ReceiptItem receiptItem)
        {
            return
            [
                new() {
                    Id = receiptItem.Id,
                    ItemName = receiptItem.ItemName,
                    ItemDiscount = receiptItem.ItemDiscount,
                    ItemPrice = receiptItem.ItemPrice,
                    Observation = receiptItem.Observation,
                    TotalPrice = receiptItem.TotalPrice,
                    Quantity = receiptItem.Quantity,
                }
            ];
        }

        public static IEnumerable<ReceiptResponse> ToReceiptResponseItems(this Receipt receipt)
        {
            return
            [
                new() {
                    EstablishmentName = receipt.EstablishmentName,
                    Id = receipt.Id,
                    Category = receipt.Category.ToCategoryResponse(),
                    ReceiptDate = receipt.ReceiptDate,
                    ReceiptItems = receipt.ReceiptItems.SelectMany(x => x.ToDomainReceiptItems()),
                    Total = receipt.Total,
                    Discount = receipt.Discount
                }
            ];
        }

        public static IEnumerable<RecurringReceiptResponse> ToRecurringReceiptResponseItems(this RecurringReceipt receipt)
        {
            return
            [
                new() {
                    EstablishmentName = receipt.EstablishmentName,
                    Id = receipt.Id,
                    Category = receipt.Category.ToCategoryResponse(),
                    DateEndRecurrence = receipt.DateEndRecurrence,
                    DateInitialRecurrence = receipt.DateInitialRecurrence,
                    Observation = receipt.Observation,
                    RecurrenceTotalPrice = receipt.RecurrenceTotalPrice
                }
            ];
        }
    }
}
