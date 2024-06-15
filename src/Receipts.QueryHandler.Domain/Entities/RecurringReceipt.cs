using MongoDB.Bson.Serialization.Attributes;

namespace Receipts.QueryHandler.Domain.Entities
{
    public class RecurringReceipt(Guid id, Guid userId, Tenant tenant, Category category, string? establishmentName, DateTime dateInitialRecurrence, DateTime dateEndRecurrence, decimal recurrenceTotalPrice, string? observation)
    {
        [BsonId]
        public Guid Id { get; set; } = id;
        public Guid UserId { get; set; } = userId;
        public Tenant Tenant { get; set; } = tenant;
        public Category Category { get; set; } = category;
        public string? EstablishmentName { get; set; } = establishmentName;
        public DateTime DateInitialRecurrence { get; set; } = dateInitialRecurrence;
        public DateTime DateEndRecurrence { get; set; } = dateEndRecurrence;
        public decimal RecurrenceTotalPrice { get; set; } = recurrenceTotalPrice;
        public string? Observation { get; set; } = observation;
    }
}
