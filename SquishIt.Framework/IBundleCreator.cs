using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace SquishIt.Framework
{
    /// <summary>
    /// Provides a mechanism for creating bundles from default values.
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
