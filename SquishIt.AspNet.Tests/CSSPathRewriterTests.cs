using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class CSSPathRewriterTests : SquishIt.Tests.CSSPathRewriterTests
    {
        public CSSPathRewriterTests() : base(TestBundleCreator.Default.HttpUtility, TestBundleCreator.Default.PathTranslator)
        {
        }
    }
}