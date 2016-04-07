using NUnit.Framework;
using SquishIt.Nancy.Web;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class CSSPathRewriterTests : SquishIt.Tests.CSSPathRewriterTests
    {
        public CSSPathRewriterTests() : base(new HttpUtility())
        {
        }
    }
}