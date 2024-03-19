using Domain.Entities;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;

namespace Domain.Interfaces
{
    public interface IReceiptRepository
    {
        Task<PagedResultFilter<Receipt>> GetVariableReceiptsAsync(ReceiptFilters queryFilter);
        Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter);
    }
}
