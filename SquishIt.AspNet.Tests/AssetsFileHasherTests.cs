using NUnit.Framework;
using SquishIt.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(TestConfiguration.Default.DefaultHttpUtility(), TestConfiguration.Default.DefaultPathTranslator())
        {
            Configuration.Instance = TestConfiguration.Default;
        }
    }
}