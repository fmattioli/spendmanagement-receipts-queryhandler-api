﻿using MongoDB.Bson.Serialization.Attributes;
using Receipts.QueryHandler.Domain.ValueObjects;

namespace Receipts.QueryHandler.Domain.Entities
{
    public class VariableReceipt(Guid id, Tenant tenant, Category category, string establishmentName, DateTime receiptDate, IEnumerable<ReceiptItem> receiptItems, decimal discount, decimal total)
    {
        [BsonId]
        public Guid Id { get; set; } = id;
        public Tenant Tenant { get; set; } = tenant;
        public Category Category { get; set; } = category;
        public string EstablishmentName { get; set; } = establishmentName;
        public DateTime ReceiptDate { get; set; } = receiptDate;
        public IEnumerable<ReceiptItem> ReceiptItems { get; set; } = receiptItems;
        public decimal Discount { get; set; } = discount;
        public decimal Total { get; set; } = total;
    }
}