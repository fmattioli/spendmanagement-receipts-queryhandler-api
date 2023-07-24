using Domain.QueriesFilters.PageFilters;

namespace Domain.QueriesFilters
{
    public class CategoriesFilters
    {
        public CategoriesFilters(IEnumerable<Guid> ids, 
            IEnumerable<string> names,
            int pageNumber,
            int pageSize)
        {
            this.Ids = ids;
            Names = names;
            PageFilter = new PageFilter(pageNumber, pageSize);
        }

        public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
        public IEnumerable<string> Names { get; set; } = new List<string>();
        public PageFilter PageFilter { get; set; }
    }
}
