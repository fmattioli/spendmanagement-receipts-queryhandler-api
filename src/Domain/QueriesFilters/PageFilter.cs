namespace Domain.Queries
{
    public class PageFilter
    {
        private int pageNumber;

        private int pageSize;

        public PageFilter()
        {
            this.pageNumber = 1;
            this.pageSize = 60;
        }

        public int PageNumber
        {
            get => this.pageNumber;
            set => this.pageNumber = (value < 1) ? 1 : value;
        }

        public int PageSize
        {
            get => this.pageSize;
            set => this.pageSize = (value < 1) ? 1 : value;
        }

        public int Skip => this.PageSize * (this.PageNumber - 1);
    }
}
