using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Domain.QueriesFilters
{
    public class RecurringReceiptFilters(
        int tenantId,
        IEnumerable<Guid>? receiptIds,
        IEnumerable<Guid>? categoryIds,
        IEnumerable<string>? establishmentNames,
        int pageNumber,
        int pageSize) : PageFilter(pageNumber, pageSize)
    {
        public int TenantId { get; set; } = tenantId;
        public IEnumerable<Guid>? ReceiptIds { get; set; } = receiptIds;
        public IEnumerable<Guid>? CategoryIds { get; set; } = categoryIds;
        public IEnumerable<string>? EstablishmentNames { get; set; } = establishmentNames;
    }
}
