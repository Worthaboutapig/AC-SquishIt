using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
    [TestFixture]
    public class MultipleWorkerProcessesTests : SquishIt.Tests.MultipleWorkerProcessesTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MultipleWorkerProcessesTests() : base(TestConfiguration.Default.DebugStatusReader)
        {
        }
    }
}
