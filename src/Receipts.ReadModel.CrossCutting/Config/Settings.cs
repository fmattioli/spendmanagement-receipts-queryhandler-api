namespace Receipts.ReadModel.CrossCutting.Config
{
    public interface ISettings
    {
        public string TokenAuth { get; }
        public TracingSettings? TracingSettings { get; }
        public MongoSettings? MongoSettings { get; }
        public SpendManagementIdentitySettings? SpendManagementIdentity { get; }
    }

    public record Settings : ISettings
    {
        public TracingSettings? TracingSettings { get; set; }
        public SpendManagementIdentitySettings? SpendManagementIdentity { get; set; }
        public string TokenAuth { get; set; } = null!;
        public MongoSettings? MongoSettings { get; set; }
    }
}
