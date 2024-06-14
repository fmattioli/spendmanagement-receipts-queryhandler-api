using Receipts.QueryHandler.Domain.Entities;
using Receipts.QueryHandler.Domain.QueriesFilters;
using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<PagedResultFilter<VariableReceipt>> GetVariableReceiptsAsync(ReceiptFilters queryFilter);
        Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter);
    }
}
