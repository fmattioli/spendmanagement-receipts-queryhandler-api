namespace SpendManagement.ReadModel.IntegrationTests.Fixtures
{
    [CollectionDefinition(nameof(SharedFixtureCollection))]
    public class SharedFixtureCollection : ICollectionFixture<MongoDBFixture>
    {
    }
}
