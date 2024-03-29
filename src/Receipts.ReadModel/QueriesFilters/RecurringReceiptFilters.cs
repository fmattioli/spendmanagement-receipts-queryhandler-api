using Receipts.ReadModel.QueriesFilters.PageFilters;

namespace Receipts.ReadModel.QueriesFilters
{
    public class RecurringReceiptFilters(
        IEnumerable<Guid> receiptIds,
        IEnumerable<Guid> categoryIds,
        IEnumerable<string> establishmentNames,
        int pageNumber,
        int pageSize) : PageFilter(pageNumber, pageSize)
    {
        public IEnumerable<Guid> ReceiptIds { get; set; } = receiptIds;
        public IEnumerable<Guid> CategoryIds { get; set; } = categoryIds;
        public IEnumerable<string> EstablishmentNames { get; set; } = establishmentNames;
    }
}
