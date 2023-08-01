using System.Collections.Generic;

namespace Web.Contracts.Receipt
{
    public class ReceiptResponse
    {
        public Guid Id { get; set; }
        public string EstablishmentName { get; set; } = null!;
        public DateTime ReceiptDate { get; set; }
        public IEnumerable<ReceiptItemResponse> ReceiptItems { get; set; } = null!;
    }

    public class ReceiptItemResponse
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string ItemName { get; set; } = null!;
        public short Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Observation { get; set; } = null!;
    }
}
