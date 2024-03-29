using Receipts.ReadModel.Queries.GetReceipts;
using Receipts.ReadModel.QueriesFilters;
using Receipts.ReadModel.QueriesFilters.PageFilters;
using Receipts.ReadModel.Entities;

namespace Receipts.ReadModel.Interfaces
{
    public interface IReceiptRepository
    {
        Task<PagedResultFilter<Receipt>> GetVariableReceiptsAsync(ReceiptFilters queryFilter);
        Task<PagedResultFilter<RecurringReceipt>> GetRecurringReceiptsAsync(RecurringReceiptFilters queryFilter);
    }
}
