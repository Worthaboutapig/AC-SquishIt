using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class ConfigurationTests : SquishIt.Tests.ConfigurationTests
    {
        public ConfigurationTests() : base(new Configuration())
        {
        }
    }
}
