using NUnit.Framework;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class ConfigurationTests : SquishIt.Tests.ConfigurationTests
    {
        public ConfigurationTests() : base(new Configuration())
        {
        }
    }
}
