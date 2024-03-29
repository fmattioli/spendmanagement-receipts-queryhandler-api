using MongoDB.Bson.Serialization.Attributes;

namespace Receipts.ReadModel.Entities
{
    public class RecurringReceipt(Guid Id, Guid categoryId, string? establishmentName, DateTime dateInitialRecurrence, DateTime dateEndRecurrence, decimal recurrenceTotalPrice, string? observation)
    {
        [BsonId]
        public Guid Id { get; set; } = Id;
        public Guid CategoryId { get; set; } = categoryId;
        public string? EstablishmentName { get; set; } = establishmentName;
        public DateTime DateInitialRecurrence { get; set; } = dateInitialRecurrence;
        public DateTime DateEndRecurrence { get; set; } = dateEndRecurrence;
        public decimal RecurrenceTotalPrice { get; set; } = recurrenceTotalPrice;
        public string? Observation { get; set; } = observation;
    }
}
