using System.Web;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Caching;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Framework.Resolvers;
using HttpContext = SquishIt.AspNet.Web.HttpContext;
using HttpUtility = SquishIt.AspNet.Web.HttpUtility;

namespace SquishIt.AspNet
{
    /// <summary>
    /// Extends the framework configuration with the System.Web-specific configuration data.
    /// </summary>
    public class DefaultBundleCreator : IBundleCreator
    {
        /// <summary>
        /// Initialise with the web framework defaults
        /// </summary>
        public DefaultBundleCreator(
            string virtualPathRoot = null,
            string physicalPathRoot = null,
            IHttpUtility httpUtility = null,
            IHttpContext httpContext = null,
            IVirtualPathUtility virtualPathUtility = null,
            IPathTranslator pathTranslator = null,
            IDebugStatusReader debugStatusReader = null,
            ITempPathProvider tempPathProvider = null,
            IFolderResolver fileSystemResolver = null,
            IFileResolver httpResolver = null,
            IFileResolver rootEmbeddedResourceResolver = null,
            IFileResolver standardEmbeddedResourceResolver = null,
            ICache cache = null,
            IContentCache bundleCache = null,
            IContentCache rawContentCache = null
            )
        {
            //    UseHttpUtility(httpUtility ?? new HttpUtility());

            //    httpContext = httpContext ?? new HttpContext(System.Web.HttpContext.Current);
            //    UseHttpContext(httpContext);

            //    virtualPathUtility = virtualPathUtility ?? new VirtualPathUtilityWrapper();
            //    UseVirtualPathUtility(virtualPathUtility);

            //    VirtualPathRoot = virtualPathRoot ?? HttpRuntime.AppDomainAppVirtualPath;
            //    PhysicalPathRoot = physicalPathRoot ?? HttpRuntime.AppDomainAppPath;

            //    pathTranslator = pathTranslator ?? new DefaultPathTranslator(VirtualPathRoot, PhysicalPathRoot, httpContext, virtualPathUtility);
            //    UsePathTranslator(pathTranslator);

            //    var machineConfigReader = new MachineConfigReader();
            //    DebugStatusReader = debugStatusReader ?? new DebugStatusReader(machineConfigReader, new HttpContext(System.Web.HttpContext.Current));

            //    UseTempPathProvider(tempPathProvider ?? new TempPathProvider());
            //    FileSystemResolver = fileSystemResolver ?? new FileSystemResolver();
            //    HttpResolver = httpResolver ?? new HttpResolver(DefaultTempPathProvider());
            //    RootEmbeddedResourceResolver = rootEmbeddedResourceResolver ?? new RootEmbeddedResourceResolver(DefaultTempPathProvider());
            //    StandardEmbeddedResourceResolver = standardEmbeddedResourceResolver ?? new StandardEmbeddedResourceResolver(DefaultTempPathProvider());

            //    cache = cache ?? new MemoryCache();
            //    BundleCache = bundleCache ?? new BundleCache(cache);
            //    RawContentCache = rawContentCache ?? new RawContentCache(cache);

            //IMachineConfigReader _machineConfigReader = new MachineConfigReader();
            //debugStatusReader = debugStatusReader ?? new DebugStatusReader(_machineConfigReader, new AspNet.Web.HttpContext(HttpContext.Current));

            //                var retryableFileOpener = new RetryableFileOpener();
            ////var bundleCss = new CSSBundle(debugStatusReader, new FileWriterFactory(retryableFileOpener, 5), new FileReaderFactory(retryableFileOpener, 5), new DirectoryWrapper(), Configuration.Instance.DefaultHasher(), Configuration.Instance.BundleCache, Configuration.Instance.RawContentCache, Configuration.Instance.DefaultHttpUtility(), Configuration.Instance.DefaultOutputBaseHref(), Configuration.Instance.DefaultPathTranslator(), Configuration.Instance.FileSystemResolver, Configuration.Instance.HttpResolver, Configuration.Instance.RootEmbeddedResourceResolver, Configuration.Instance.StandardEmbeddedResourceResolver, Configuration.Instance.VirtualPathRoot);
            //var machineConfigReader = new MachineConfigReader();
            //var httpContext = new AspNet.Web.HttpContext(HttpContext.Current);
            //var debugStatusReader = new DebugStatusReader(machineConfigReader, httpContext, new TrustLevel());
        //IHasher _hasher = new Hasher(new RetryableFileO
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public JavaScriptBundle GetJavaScriptBundle()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CSSBundle GetCssBundle()
        {
            throw new System.NotImplementedException();
        }
    }
}