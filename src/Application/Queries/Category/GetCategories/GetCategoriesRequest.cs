using Microsoft.AspNetCore.Mvc;

namespace Application.Queries.Category.GetCategories
{
    public class GetCategoriesRequest
    {
        public GetCategoriesRequest()
        {
            PageFilter = new PageFilterRequest { Page = 1, PageSize = 60, };
        }

        [BindProperty(Name = "")]
        public PageFilterRequest PageFilter { get; set; }

        [FromQuery(Name = "categoryIds")]
        public IEnumerable<Guid> CategoryIds { get; set; } = new List<Guid>();
        [FromQuery(Name = "categoryNames")]
        public IEnumerable<string> CategoryNames { get; set; } = new List<string>();
    }

    public record PageFilterRequest
    {
        private const int LowerBoundPageNumber = 1;

        private int page;

        private int pageSize = 60;

        [FromQuery(Name = "page")]
        public int Page
        {
            get => page;
            set => page = value < LowerBoundPageNumber ? LowerBoundPageNumber : value;
        }

        [FromQuery(Name = "pageSize")]
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value < 1 ? 1 : value;
        }
    }
}
