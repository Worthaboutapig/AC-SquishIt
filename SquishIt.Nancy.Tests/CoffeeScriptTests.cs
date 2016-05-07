using NUnit.Framework;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class CoffeeScriptTests : SquishIt.Tests.CoffeeScriptTests
    {
        public CoffeeScriptTests() : base(
            TestConfiguration.Default.DefaultOutputBaseHref(),
            TestConfiguration.Default.DefaultPathTranslator(),
            TestConfiguration.Default.FileSystemResolver,
            TestConfiguration.Default.HttpResolver,
            TestConfiguration.Default.RootEmbeddedResourceResolver,
            TestConfiguration.Default.StandardEmbeddedResourceResolver,
            TestConfiguration.Default.VirtualPathRoot, TODO, TODO)
        {
        }
    }

}