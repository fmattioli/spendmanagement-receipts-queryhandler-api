namespace Domain.QueriesFilters.PageFilters
{
    public class PagedResultFilter<T>
    {
        public int PageNumber { get; set; }
        public int PageSize => Results.Count();
        public int PageSizeLimit { get; set; }
        public int TotalPages => (TotalResults + PageSizeLimit - 1) / PageSizeLimit;
        public int TotalResults { get; set; }
        public IEnumerable<T> Results { get; set; } = null!;
    }
}
