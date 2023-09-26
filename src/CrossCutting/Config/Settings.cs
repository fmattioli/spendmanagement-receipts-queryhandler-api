using CrossCutting.Config;

namespace Crosscutting.Cofig
{
    public interface ISettings
    {
        public string TokenAuth { get; }
        public TracingSettings? TracingSettings { get; }
        public MongoSettings MongoSettings { get; }
        public SpendManagementIdentitySettings SpendManagementIdentity { get; }
    }

    public record Settings : ISettings
    {
        public TracingSettings? TracingSettings { get; set; }
        public SpendManagementIdentitySettings SpendManagementIdentity { get; set; } = null!;
        public string TokenAuth { get; set; } = null!;
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
