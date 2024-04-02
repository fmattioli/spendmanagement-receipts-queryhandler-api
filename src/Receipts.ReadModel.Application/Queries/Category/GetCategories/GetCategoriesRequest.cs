using Microsoft.AspNetCore.Mvc;

namespace Receipts.ReadModel.Application.Queries.Category.GetCategories
{
    public record GetCategoriesRequest
    {
        public GetCategoriesRequest()
        {
            PageFilter = new PageFilterRequest { PageNumber = 1, PageSize = 60, };
        }

        [FromQuery]
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

        public int PageNumber
        {
            get => page;
            set => page = value < LowerBoundPageNumber ? LowerBoundPageNumber : value;
        }

        public int PageSize
        {
            get => pageSize;
            set => pageSize = value < 1 ? 1 : value;
        }
    }
}
