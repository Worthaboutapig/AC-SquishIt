using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(TestConfiguration.Default.DefaultHttpUtility(), TestConfiguration.Default.DefaultPathTranslator())
        {
        }
    }
}