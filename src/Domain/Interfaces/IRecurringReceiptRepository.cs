using Domain.Entities;
using Domain.QueriesFilters;
using Domain.QueriesFilters.PageFilters;

namespace Domain.Interfaces
{
    public interface IRecurringReceiptRepository
    {
        Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter);
    }
}
