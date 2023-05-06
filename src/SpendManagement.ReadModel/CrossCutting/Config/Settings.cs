using Crosscutting.Config;

namespace Crosscutting.Cofig
{
    public interface ISettings
    {
        public MongoSettings MongoSettings { get; }
    }

    public record Settings : ISettings
    {
        public MongoSettings MongoSettings { get; set; } = null!;
    }
}
