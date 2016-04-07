using NUnit.Framework;
using SquishIt.Nancy.Web;

namespace SquishIt.Nancy.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(new HttpUtility())
        {
        }
    }
}
