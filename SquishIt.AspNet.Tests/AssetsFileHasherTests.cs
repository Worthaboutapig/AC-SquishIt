using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class AssetsFileHasherTests : SquishIt.Tests.AssetsFileHasherTests
    {
        public AssetsFileHasherTests() : base(TestBundleCreator.Default.HttpUtility, TestBundleCreator.Default.PathTranslator)
        {
        }
    }
}