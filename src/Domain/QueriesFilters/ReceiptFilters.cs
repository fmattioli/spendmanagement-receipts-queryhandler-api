using Domain.QueriesFilters.PageFilters;

namespace Domain.Queries.GetReceipts
{
    public class ReceiptFilters(IEnumerable<Guid> receiptIds,
        IEnumerable<Guid> categoryIds,
        IEnumerable<Guid> receiptItemIds,
        IEnumerable<string> establishmentNames,
        IEnumerable<string> itemNames,
        DateTime receiptDate,
        DateTime receiptDateFinal,
        int pageNumber,
        int pageSize) : PageFilter(pageNumber, pageSize)
    {
        public IEnumerable<Guid> ReceiptIds { get; set; } = receiptIds;
        public IEnumerable<Guid> CategoryIds { get; set; } = categoryIds;
        public IEnumerable<Guid> ReceiptItemIds { get; set; } = receiptItemIds;
        public IEnumerable<string> EstablishmentNames { get; set; } = establishmentNames;
        public IEnumerable<string> ReceiptItemNames { get; set; } = itemNames;
        public DateTime? ReceiptDate { get; set; } = receiptDate;
        public DateTime? ReceiptDateFinal { get; set; } = receiptDateFinal;
    }
}
