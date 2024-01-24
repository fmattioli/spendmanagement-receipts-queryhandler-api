using Microsoft.AspNetCore.Mvc;

namespace Application.Queries.Receipt.GetReceipts
{
    public record GetReceiptsRequest
    {
        public GetReceiptsRequest()
        {
            PageFilter = new PageFilterRequest { Page = 1, PageSize = 60, };
        }

        [BindProperty(Name = "")]
        public PageFilterRequest PageFilter { get; set; }

        [FromQuery]
        public IEnumerable<Guid> ReceiptIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<Guid> CategoryIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<string> EstablishmentNames { get; set; } = new List<string>();

        [FromQuery]
        public DateTime ReceiptDate { get; set; }

        [FromQuery]
        public DateTime ReceiptDateFinal { get; set; }

        [FromQuery]
        public IEnumerable<Guid> ReceiptItemIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<string> ReceiptItemNames { get; set; } = new List<string>();
    }

    public record PageFilterRequest
    {
        private const int LowerBoundPageNumber = 1;

        private int page;

        private int pageSize = 60;

        [FromQuery(Name = "page")]
        internal int Page
        {
            get => page;
            set => page = value < LowerBoundPageNumber ? LowerBoundPageNumber : value;
        }

        [FromQuery(Name = "pageSize")]
        internal int PageSize
        {
            get => pageSize;
            set => pageSize = value < 1 ? 1 : value;
        }
    }
}
