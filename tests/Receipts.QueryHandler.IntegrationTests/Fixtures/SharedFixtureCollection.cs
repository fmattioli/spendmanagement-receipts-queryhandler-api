namespace Receipts.QueryHandler.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(SharedFixtureCollection))]
    public class SharedFixtureCollection : ICollectionFixture<MongoDBFixture>
    {
    }
}
