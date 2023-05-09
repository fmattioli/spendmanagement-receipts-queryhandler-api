namespace Domain.Queries.GetReceipts
{
    public class GetReceiptsFilter
    {
        public GetReceiptsFilter(IEnumerable<Guid> receiptIds, 
            IEnumerable<Guid> receiptItemIds, 
            IEnumerable<string> establishmentNames, 
            DateTime receiptDate, 
            DateTime receiptDateFinal)
        {
            ReceiptIds = receiptIds;
            ReceiptItemIds = receiptItemIds;
            EstablishmentNames = establishmentNames;
            ReceiptDate = receiptDate;
            ReceiptDateFinal = receiptDateFinal;
            PageFilter = new PageFilter();
        }

        public IEnumerable<Guid> ReceiptIds { get; set; }
        public IEnumerable<Guid> ReceiptItemIds { get; set; }
        public IEnumerable<string> EstablishmentNames { get; set; }
        public DateTime ReceiptDate { get; set; }
        public DateTime ReceiptDateFinal { get; set; }
        public PageFilter PageFilter { get; set; }
    }
}
