using SquishIt.Framework;
using SquishIt.Framework.Web;

namespace SquishIt.Tests.Helpers
{
    /// <summary>
    /// Adds methods for getting bundle factories.
    /// </summary>
    public interface ITestBundleCreator : IBundleCreator
    {
        JavaScriptBundleFactory JavaScriptBundleFactory { get; }
        CssBundleFactory CssBundleFactory { get; }
    }
}
