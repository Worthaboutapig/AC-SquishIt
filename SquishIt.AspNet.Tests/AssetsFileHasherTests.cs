using NUnit.Framework;
using SquishIt.AspNet.Web;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(new HttpUtility())
        {
        }
    }
}
