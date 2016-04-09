using NUnit.Framework;
using SquishIt.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class CSSPathRewriterTests : SquishIt.Tests.CSSPathRewriterTests
    {
        public CSSPathRewriterTests() : base(TestConfiguration.Default.DefaultHttpUtility(), TestConfiguration.Default.DefaultPathTranslator())
        {
            Configuration.Instance = TestConfiguration.Default;
        }
    }
}