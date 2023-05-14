namespace Domain.Queries
{
    public class PageFilter
    {
        public PageFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int Skip => this.PageSize * (this.PageNumber - 1);
    }
}
