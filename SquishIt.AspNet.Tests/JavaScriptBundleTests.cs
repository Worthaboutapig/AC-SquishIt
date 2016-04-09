using NUnit.Framework;
using SquishIt.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class JavaScriptBundleTests : SquishIt.Tests.JavaScriptBundleTests
    {
        public JavaScriptBundleTests() : base(
            TestConfiguration.Default.DefaultOutputBaseHref(),
            TestConfiguration.Default.DefaultPathTranslator(),
            TestConfiguration.Default.FileSystemResolver,
            TestConfiguration.Default.HttpResolver,
            TestConfiguration.Default.RootEmbeddedResourceResolver,
            TestConfiguration.Default.StandardEmbeddedResourceResolver)
        {
            Configuration.Instance = TestConfiguration.Default;
        }
    }
}