using Contracts.Web.Http.Common;
using Contracts.Web.Http.Receipt.Requests;
using Contracts.Web.Http.Receipt.Responses;
using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;
using Receipts.QueryHandler.Domain.ValueObjects;

namespace Receipts.QueryHandler.Application.Mappers
{
    public static class ReceiptMapper
    {
        public static ReceiptFilters ToDomainFilters(this GetVariableReceiptsRequest getVariableReceiptsInput, int tenantId)
        {
            return new ReceiptFilters(
                tenantId,
                getVariableReceiptsInput.ReceiptIds,
                getVariableReceiptsInput.CategoryIds,
                getVariableReceiptsInput.ReceiptItemIds,
                getVariableReceiptsInput.EstablishmentNames,
                getVariableReceiptsInput.ReceiptItemNames,
                getVariableReceiptsInput.ReceiptDateInitial,
                getVariableReceiptsInput.ReceiptDateEnd,
                getVariableReceiptsInput.PageFilter.Page,
                getVariableReceiptsInput.PageFilter.PageSize);
        }

        public static RecurringReceiptFilters ToDomainFilters(this GetRecurringReceiptsRequest getReceiptsInput, int tenantId)
        {
            return new RecurringReceiptFilters(
                tenantId,
                getReceiptsInput.ReceiptIds,
                getReceiptsInput.CategoryIds,
                getReceiptsInput.EstablishmentNames,
                getReceiptsInput.PageFilter.Page,
                getReceiptsInput.PageFilter.PageSize);
        }

        public static PagedResult<GetVariableReceiptResponse> ToResponse(this PagedResultFilter<Receipt> receipts, PageFilterRequest pageFilter)
        {
            return new PagedResult<GetVariableReceiptResponse>
            {
                PageNumber = pageFilter.Page,
                PageSize = pageFilter.PageSize,
                Results = receipts.Results.SelectMany(x => x.ToReceiptResponseItems()),
                TotalAmount = receipts.ReceiptsTotalAmount,
                TotalPages = receipts.TotalPages,
                TotalResults = receipts.TotalResults
            };
        }

        public static PagedResult<GetRecurringReceiptResponse> ToResponse(this PagedResultFilter<RecurringReceipt> recurringReceipts, PageFilterRequest pageFilter)
        {
            return new PagedResult<GetRecurringReceiptResponse>
            {
                PageNumber = pageFilter.Page,
                PageSize = pageFilter.PageSize,
                Results = recurringReceipts.Results.SelectMany(x => x.ToRecurringReceiptResponseItems()),
                TotalPages = recurringReceipts.TotalPages,
                TotalResults = recurringReceipts.TotalResults,
                TotalAmount = recurringReceipts.ReceiptsTotalAmount
            };
        }

        public static GetVariableReceiptResponse ToReceiptResponse(this Receipt receipt)
        {
            return new GetVariableReceiptResponse
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

        public static IEnumerable<GetReceiptItemResponse> ToDomainReceiptItems(this ReceiptItem receiptItem)
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

        public static IEnumerable<GetVariableReceiptResponse> ToReceiptResponseItems(this Receipt receipt)
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

        public static IEnumerable<GetRecurringReceiptResponse> ToRecurringReceiptResponseItems(this RecurringReceipt receipt)
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
