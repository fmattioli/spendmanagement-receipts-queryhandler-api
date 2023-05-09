using Domain.Entities;
using Domain.Queries;
using Domain.Queries.GetReceipts;

namespace Domain.Interfaces
{
    public interface IReceiptRepository : IBaseRepository<Receipt>
    {
        Task<PagedResultFilter<Receipt>> GetReceiptsAsync(GetReceiptsFilter queryFilter);
    }
}
