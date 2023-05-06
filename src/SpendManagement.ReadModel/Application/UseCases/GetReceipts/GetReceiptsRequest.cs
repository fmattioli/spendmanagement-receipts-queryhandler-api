using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsRequest
    {
        [FromQuery]
        public IEnumerable<Guid> ReceiptIds { get; set; } = null!;

        [FromQuery]
        public IEnumerable<string> EstablishmentNames { get; set; } = null!;

        [FromQuery]
        public DateTime ReceiptDate { get; set; }

        [FromQuery]
        public DateTime ReceiptDateFinal { get; set; }
    }
}
