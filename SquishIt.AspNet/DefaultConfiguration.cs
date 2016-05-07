using System.Web;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Caching;
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
    public class DefaultConfiguration : Configuration
    {
        /// <summary>
        /// Initialise with the web framework defaults
        /// </summary>
        public DefaultConfiguration(
            string virtualPathRoot,
            string physicalPathRoot,
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
            UseHttpUtility(httpUtility ?? new HttpUtility());

            httpContext = httpContext ?? new HttpContext(System.Web.HttpContext.Current);
            UseHttpContext(httpContext);

            virtualPathUtility = virtualPathUtility ?? new VirtualPathUtilityWrapper();
            UseVirtualPathUtility(virtualPathUtility);

            VirtualPathRoot = virtualPathRoot ?? HttpRuntime.AppDomainAppVirtualPath;
            PhysicalPathRoot = physicalPathRoot ?? HttpRuntime.AppDomainAppPath;

            pathTranslator = pathTranslator ?? new DefaultPathTranslator(VirtualPathRoot, PhysicalPathRoot, httpContext, virtualPathUtility);
            UsePathTranslator(pathTranslator);

            var machineConfigReader = new MachineConfigReader();
            DebugStatusReader = debugStatusReader ?? new DebugStatusReader(machineConfigReader, new HttpContext(System.Web.HttpContext.Current));

            UseTempPathProvider(tempPathProvider ?? new TempPathProvider());
            FileSystemResolver = fileSystemResolver ?? new FileSystemResolver();
            HttpResolver = httpResolver ?? new HttpResolver(DefaultTempPathProvider());
            RootEmbeddedResourceResolver = rootEmbeddedResourceResolver ?? new RootEmbeddedResourceResolver(DefaultTempPathProvider());
            StandardEmbeddedResourceResolver = standardEmbeddedResourceResolver ?? new StandardEmbeddedResourceResolver(DefaultTempPathProvider());

            cache = cache ?? new MemoryCache();
            BundleCache = bundleCache ?? new BundleCache(cache);
            RawContentCache = rawContentCache ?? new RawContentCache(cache);
        }
    }
}