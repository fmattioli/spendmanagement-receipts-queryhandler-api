using Domain.QueriesFilters.PageFilters;

namespace Domain.QueriesFilters
{
    public class CategoryFilters
    {
        public CategoryFilters(IEnumerable<Guid> categoryIds, IEnumerable<string> categoryNames, short pageNumber, short pageSize)
        {
            CategoryIds = categoryIds;
            CategoryNames = categoryNames;
            PageFilter = new PageFilter(pageNumber, pageSize);
        }

        public IEnumerable<Guid> CategoryIds { get; set; } = new List<Guid>();

        public IEnumerable<string> CategoryNames { get; set; } = new List<string>();

        public PageFilter PageFilter { get; set; }
    }
}
