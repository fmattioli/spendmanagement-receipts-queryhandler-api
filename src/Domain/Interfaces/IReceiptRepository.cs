using Domain.Entities;
using Domain.Queries.GetReceipts;
using Domain.QueriesFilters.PageFilters;

namespace Domain.Interfaces
{
    public interface IReceiptRepository : IBaseRepository<Receipt>
    {
        Task<PagedResultFilter<Receipt>> GetReceiptsAsync(ReceiptFilters queryFilter);
    }
}
