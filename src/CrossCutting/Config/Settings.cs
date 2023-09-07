using Crosscutting.Config;

namespace Crosscutting.Cofig
{
    public interface ISettings
    {
        string TokenAuth { get; }
        public MongoSettings MongoSettings { get; }
        public SpendManagementIdentitySettings SpendManagementIdentity { get; }
    }

    public record Settings : ISettings
    {
        public SpendManagementIdentitySettings SpendManagementIdentity { get; set; } = null!;
        public string TokenAuth { get; set; } = null!;
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
