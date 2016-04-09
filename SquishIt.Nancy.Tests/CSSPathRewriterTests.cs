using NUnit.Framework;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class CSSPathRewriterTests : SquishIt.Tests.CSSPathRewriterTests
    {
        public CSSPathRewriterTests() : base(TestConfiguration.Default.DefaultHttpUtility(), TestConfiguration.Default.DefaultPathTranslator())
        {
        }
    }
}