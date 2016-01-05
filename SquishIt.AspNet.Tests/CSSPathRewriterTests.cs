using NUnit.Framework;
using SquishIt.AspNet.Web;

namespace SquishIt.AspNet.Tests
{

    [TestFixture]
    public class CSSPathRewriterTests : SquishIt.Tests.CSSPathRewriterTests
    {
        public CSSPathRewriterTests() : base(new HttpUtility())
        {
        }
    }
}
