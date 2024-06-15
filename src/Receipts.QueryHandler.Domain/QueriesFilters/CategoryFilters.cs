﻿using Receipts.QueryHandler.Domain.QueriesFilters.PageFilters;

namespace Receipts.QueryHandler.Domain.QueriesFilters
{
    public class CategoryFilters(
        int tenantId,
        IEnumerable<Guid>? categoryIds,
        IEnumerable<string>? categoryNames,
        short pageNumber,
        short pageSize) : PageFilter(pageNumber, pageSize)
    {
        public int TenantId { get; set; } = tenantId;
        public IEnumerable<Guid>? CategoryIds { get; set; } = categoryIds;

        public IEnumerable<string>? CategoryNames { get; set; } = categoryNames;
    }
}
