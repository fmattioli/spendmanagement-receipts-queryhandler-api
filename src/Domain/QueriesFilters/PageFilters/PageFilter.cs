namespace Domain.QueriesFilters.PageFilters
{
    public class PageFilter
    {
        public PageFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Skip => PageSize * (PageNumber - 1);
    }
}
