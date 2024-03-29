using Receipts.ReadModel.QueriesFilters.PageFilters;

namespace Receipts.ReadModel.QueriesFilters
{
    public class CategoryFilters : PageFilter
    {
        public CategoryFilters(IEnumerable<Guid> categoryIds, 
            IEnumerable<string> categoryNames, 
            short pageNumber, 
            short pageSize) : base(pageNumber, pageSize)
        {
            CategoryIds = categoryIds;
            CategoryNames = categoryNames;
        }

        public IEnumerable<Guid> CategoryIds { get; set; } = new List<Guid>();

        public IEnumerable<string> CategoryNames { get; set; } = new List<string>();
    }
}
