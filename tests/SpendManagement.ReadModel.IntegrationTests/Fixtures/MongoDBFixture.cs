using AutoFixture;

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using SpendManagement.ReadModel.IntegrationTests.Configuration;

namespace SpendManagement.ReadModel.IntegrationTests.Fixtures
{
    public class MongoDBFixture : IAsyncLifetime
    {
        public readonly IMongoDatabase database;
        private readonly List<Guid> categoryIds = [];
        private readonly List<Guid> receiptIds = [];

        public MongoDBFixture()
        {
            var mongoUrl = new MongoUrl(TestSettings.MongoSettings?.ConnectionString);
            this.database = new MongoClient(mongoUrl).GetDatabase(TestSettings.MongoSettings?.Database);
        }

        public async Task DisposeAsync()
        {
            if (categoryIds.Count != 0)
            {
                var collection = this.database.GetCollection<Category>("Categories");

                var filter = new FilterDefinitionBuilder<Category>()
                    .In(x => x.Id, categoryIds);

                await collection.DeleteManyAsync(filter);
            }

            if (receiptIds.Count != 0)
            {
                var collection = this.database.GetCollection<Receipt>("Receipts");

                var filter = new FilterDefinitionBuilder<Receipt>()
                    .In(x => x.Id, receiptIds);

                await collection.DeleteManyAsync(filter);
            }
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InsertReceiptAsync(params Receipt[] receipts)
        {
            var collection = this.database.GetCollection<Receipt>("Receipts");
            await Task.WhenAll(receipts.Select(receipt => collection.InsertOneAsync(receipt)));
            receiptIds.AddRange(receipts.Select(x => x.Id));
        }

        public async Task InsertCategoryAsync(Category category)
        {
            var collection = this.database.GetCollection<Category>("Categories");
            await collection.InsertOneAsync(category);
            this.categoryIds.Add(category.Id);
        }
    }

    public record Category([property: BsonId] Guid Id, string? Name, DateTime CreatedDate);

    public record Receipt([property: BsonId] Guid Id, string? EstablishmentName, DateTime ReceiptDate, IEnumerable<ReceiptItem>? ReceiptItems);

    public record ReceiptItem(Guid Id, Guid CategoryId, string ItemName, short Quantity, decimal ItemPrice, decimal TotalPrice, string Observation);
}
