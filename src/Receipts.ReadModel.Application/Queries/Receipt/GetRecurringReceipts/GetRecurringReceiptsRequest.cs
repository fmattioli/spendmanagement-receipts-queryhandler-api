using Microsoft.AspNetCore.Mvc;

using Receipts.ReadModel.Application.Queries.Common;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetRecurringReceipts
{
    public class GetRecurringReceiptsRequest
    {
        public GetRecurringReceiptsRequest()
        {
            PageFilter = new PageFilterRequest { Page = 1, PageSize = 60, };
        }

        [FromQuery]
        public PageFilterRequest PageFilter { get; set; }

        [FromQuery]
        public IEnumerable<Guid> ReceiptIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<Guid> CategoryIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<string> EstablishmentNames { get; set; } = new List<string>();
    }
}
