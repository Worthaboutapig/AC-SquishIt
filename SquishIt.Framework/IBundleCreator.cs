using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace SquishIt.Framework
{
    /// <summary>
    /// Creates new bundles.
    /// </summary>
    public interface IBundleCreator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        JavaScriptBundle GetJavaScriptBundle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        CSSBundle GetCssBundle();
    }
}
