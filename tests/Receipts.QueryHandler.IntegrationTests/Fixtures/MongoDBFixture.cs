using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Receipts.QueryHandler.IntegrationTests.Configuration;

namespace Receipts.QueryHandler.IntegrationTests.Fixtures
{
    public class MongoDBFixture : IAsyncLifetime
    {
        public readonly IMongoDatabase database;
        private readonly List<Guid> categoryIds = [];
        private readonly List<Guid> receiptIds = [];
        private readonly List<Guid> recurringReceiptIds = [];

        public MongoDBFixture()
        {
            var mongoUrl = new MongoUrl(TestSettings.MongoSettings!.ConnectionString);
            database = new MongoClient(mongoUrl).GetDatabase(TestSettings.MongoSettings.Database);
        }

        public async Task DisposeAsync()
        {
            if (categoryIds.Count != 0)
            {
                var collection = database.GetCollection<Category>("Categories");

                var filter = new FilterDefinitionBuilder<Category>()
                    .In(x => x.Id, categoryIds);

                await collection.DeleteManyAsync(filter);
            }

            if (receiptIds.Count != 0)
            {
                var collection = database.GetCollection<Receipt>("Receipts");

                var filter = new FilterDefinitionBuilder<Receipt>()
                    .In(x => x.Id, receiptIds);

                await collection.DeleteManyAsync(filter);
            }

            if (recurringReceiptIds.Count != 0)
            {
                var collection = database.GetCollection<RecurringReceipt>("RecurringReceipts");

                var filter = new FilterDefinitionBuilder<RecurringReceipt>()
                    .In(x => x.Id, receiptIds);

                await collection.DeleteOneAsync(filter);
            }
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InsertReceiptAsync(params Receipt[] receipts)
        {
            var collection = database.GetCollection<Receipt>("Receipts");
            await Task.WhenAll(receipts.Select(receipt => collection.InsertOneAsync(receipt)));
            receiptIds.AddRange(receipts.Select(x => x.Id));
        }

        public async Task InsertCategoryAsync(Category category)
        {
            var collection = database.GetCollection<Category>("Categories");
            await collection.InsertOneAsync(category);
            categoryIds.Add(category.Id);
        }

        public async Task InsertRecurringReceiptAsync(RecurringReceipt recurringReceipt)
        {
            var collection = database.GetCollection<RecurringReceipt>("RecurringReceipts");
            await collection.InsertOneAsync(recurringReceipt);
            recurringReceiptIds.Add(recurringReceipt.Id);
        }
    }

    public record Category([property: BsonId] Guid Id, string? Name, DateTime CreatedDate);

    public record Receipt([property: BsonId] Guid Id, Guid CategoryId, string? EstablishmentName, DateTime ReceiptDate, IEnumerable<ReceiptItem>? ReceiptItems, decimal Discount, decimal Total);

    public record ReceiptItem(Guid Id, string ItemName, short Quantity, decimal ItemPrice, decimal TotalPrice, string Observation);

    public record RecurringReceipt(Guid Id, Guid CategoryId, string? EstablishmentName, DateTime DateInitialRecurrence, DateTime DateEndRecurrence, decimal RecurrenceTotalPrice, string? Observation);
}
