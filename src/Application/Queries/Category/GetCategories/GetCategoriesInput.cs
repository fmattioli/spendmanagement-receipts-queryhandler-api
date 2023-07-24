using Application.Queries.Common;

namespace Application.Queries.Category.GetCategories
{
    public class GetCategoriesInput
    {
        public IEnumerable<Guid> Ids { get; set; } = new List<Guid>();
        public IEnumerable<Guid> Names { get; set; } = new List<Guid>();
        public PageFilter PageFilter { get; set; } = null!;
    }
}
