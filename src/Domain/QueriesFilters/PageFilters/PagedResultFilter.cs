using System.Text.Json.Serialization;

namespace Domain.QueriesFilters.PageFilters
{
    public class PagedResultFilter<T>
    {
        public int PageSize { get; set; }

        public int TotalPages => (TotalResults + PageSize - 1) / PageSize;

        public int TotalResults { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public decimal ReceiptsTotalAmount { get; set; }

        public IEnumerable<T> Results { get; set; } = null!;
    }
}
