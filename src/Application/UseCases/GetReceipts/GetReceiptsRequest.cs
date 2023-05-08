using Microsoft.AspNetCore.Mvc;

namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsRequest
    {
        /// <summary>
        /// Filter by receiptIds
        /// <param name="ReceiptIds">Filter by Product numbers</param>
        /// </summary>
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
