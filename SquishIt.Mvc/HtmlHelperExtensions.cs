using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Files;

namespace SquishIt.Mvc
{
    public static class HtmlHelperExtensions
    {
        private static readonly IMachineConfigReader _machineConfigReader = new MachineConfigReader();

        public static CSSBundle BundleCss(this HtmlHelper html, IDebugStatusReader debugStatusReader = null)
        {
            debugStatusReader = debugStatusReader ?? new DebugStatusReader(_machineConfigReader, new AspNet.Web.HttpContext(HttpContext.Current));
	        var retryableFileOpener = new RetryableFileOpener();
	        var bundleCss = new CSSBundle(debugStatusReader, new FileWriterFactory(retryableFileOpener, 5), new FileReaderFactory(retryableFileOpener, 5), new DirectoryWrapper(), Configuration.Instance.DefaultHasher(), Configuration.Instance.BundleCache, Configuration.Instance.RawContentCache, Configuration.Instance.DefaultHttpUtility(), Configuration.Instance.DefaultOutputBaseHref(), Configuration.Instance.DefaultPathTranslator(), Configuration.Instance.FileSystemResolver, Configuration.Instance.HttpResolver, Configuration.Instance.RootEmbeddedResourceResolver, Configuration.Instance.StandardEmbeddedResourceResolver, Configuration.Instance.VirtualPathRoot);

            return bundleCss;
        }

        public static JavaScriptBundle BundleJavaScript(this HtmlHelper html, IDebugStatusReader debugStatusReader = null)
        {
            debugStatusReader = debugStatusReader ?? new DebugStatusReader(_machineConfigReader, new AspNet.Web.HttpContext(HttpContext.Current));
            return new JavaScriptBundle(debugStatusReader);
        }

        public static string ViewName(this HtmlHelper html)
        {
            var webPage = html.ViewDataContainer as WebPageBase;
            return webPage.VirtualPath;
        }
        public static void AddResources(this HtmlHelper html, params string[] resourceFiles)
        {
            AutoBundler.Current.AddResources(html.ViewName(),resourceFiles);
        }

        public static void AddStyleResources(this HtmlHelper html, params string[] resourceFiles)
        {
            AutoBundler.Current.AddStyleResources(html.ViewName(), resourceFiles);
        }

        public static void AddScriptResources(this HtmlHelper html, params string[] resourceFiles)
        {
            AutoBundler.Current.AddScriptResources(html.ViewName(), resourceFiles);
        }

        public static HtmlString ResourceLinks(this HtmlHelper html)
        {
            return new HtmlString(AutoBundler.Current.StyleResourceLinks + AutoBundler.Current.ScriptResourceLinks);
        }

        public static HtmlString StyleResourceLinks(this HtmlHelper html)
        {
            return new HtmlString(AutoBundler.Current.StyleResourceLinks);
        }

        public static HtmlString ScriptResourceLinks(this HtmlHelper html)
        {
            return new HtmlString(AutoBundler.Current.ScriptResourceLinks);
        }
    }
}
