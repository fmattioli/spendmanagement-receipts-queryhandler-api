using Receipts.ReadModel.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace Receipts.ReadModel.Entities
{
    public class Receipt(Guid id, Category category, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems, decimal discount, decimal total)
    {
        [BsonId]
        public Guid Id { get; set; } = id;
        public Category Category { get; set; } = category;
        public string EstablishmentName { get; set; } = establishmentName;
        public DateTime ReceiptDate { get; set; } = receiptDate;
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; } = receiptItems;
        public decimal Discount { get; set; } = discount;
        public decimal Total { get; set; } = total;
    }
}
