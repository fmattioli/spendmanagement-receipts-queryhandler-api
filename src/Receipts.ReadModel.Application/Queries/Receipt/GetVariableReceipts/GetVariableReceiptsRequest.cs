using Microsoft.AspNetCore.Mvc;

using Receipts.ReadModel.Application.Queries.Common;

namespace Receipts.ReadModel.Application.Queries.Receipt.GetVariableReceipts
{
    public record GetVariableReceiptsRequest
    {
        public GetVariableReceiptsRequest()
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

        [FromQuery]
        public DateTime ReceiptDate { get; set; }

        [FromQuery]
        public DateTime ReceiptDateFinal { get; set; }

        [FromQuery]
        public IEnumerable<Guid> ReceiptItemIds { get; set; } = new List<Guid>();

        [FromQuery]
        public IEnumerable<string> ReceiptItemNames { get; set; } = new List<string>();
    }
}
