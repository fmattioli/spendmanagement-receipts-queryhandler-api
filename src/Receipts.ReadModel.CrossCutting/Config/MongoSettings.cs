namespace Receipts.ReadModel.CrossCutting.Config
{
    public record MongoSettings
    {
        public string Database { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
