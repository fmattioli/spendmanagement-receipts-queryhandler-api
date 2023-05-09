namespace Domain.Queries
{
    public class PagedResultFilter<T>
    {
        public int PageNumber { get; set; }
        public int PageSize => Results.Count();
        public int PageSizeLimit { get; set; }
        public int TotalPages => (this.TotalResults + this.PageSizeLimit - 1) / this.PageSizeLimit;
        public int TotalResults { get; set; }
        public IEnumerable<T> Results { get; set; } = null!;
    }
}
