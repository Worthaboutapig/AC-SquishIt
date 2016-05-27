using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class CoffeeScriptTests : SquishIt.Tests.CoffeeScriptTests
    {
        public CoffeeScriptTests() : base(() => TestBundleCreator.Default.JavaScriptBundleFactory, TestBundleCreator.Default.PathTranslator)
        {
        }
    }

}