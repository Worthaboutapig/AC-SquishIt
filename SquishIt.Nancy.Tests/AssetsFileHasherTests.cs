using NUnit.Framework;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(TestConfiguration.Default.DefaultHttpUtility(), TestConfiguration.Default.DefaultPathTranslator())
        {
        }
    }
}
