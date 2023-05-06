namespace Application.UseCases.GetReceipts
{
    public class GetReceiptsResponse
    {
        public Guid Id { get; set; }
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; } = null!;
    }

    public record ReceiptItem
    {
        public Guid Id { get; set; }
        public Guid ReceiptId { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get { return Quantity * ItemPrice; } }
        public string Observation { get; set; } = null!;
    }
}
