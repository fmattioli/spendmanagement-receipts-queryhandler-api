﻿namespace Receipts.QueryHandler.Domain.QueriesFilters.PageFilters
{
    public class PageFilter(int pageNumber, int pageSize)
    {
        public int PageNumber { get; set; } = pageNumber;
        public int PageSize { get; set; } = pageSize;
        public int Skip => PageSize * (PageNumber - 1);
    }
}
