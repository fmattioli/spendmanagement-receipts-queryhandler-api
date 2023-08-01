using Domain.QueriesFilters.PageFilters;

namespace Domain.Queries.GetReceipts
{
    public class ReceiptFilters : PageFilter
    {
        public ReceiptFilters(IEnumerable<Guid> receiptIds, 
            IEnumerable<Guid> receiptItemIds,
            IEnumerable<string> establishmentNames,
            IEnumerable<string> itemNames,
            DateTime receiptDate,
            DateTime receiptDateFinal,
            int pageNumber,
            int pageSize) : base(pageNumber, pageSize)
        {
            ReceiptIds = receiptIds;
            ReceiptItemIds = receiptItemIds;
            EstablishmentNames = establishmentNames;
            ReceiptDate = receiptDate;
            ReceiptDateFinal = receiptDateFinal;
            ItemNames = itemNames;
        }

        public IEnumerable<Guid> ReceiptIds { get; set; }
        public IEnumerable<Guid> ReceiptItemIds { get; set; }
        public IEnumerable<string> EstablishmentNames { get; set; }
        public IEnumerable<string> ItemNames { get; set; }
        public DateTime? ReceiptDate { get; set; }
        public DateTime? ReceiptDateFinal { get; set; }
    }
}
