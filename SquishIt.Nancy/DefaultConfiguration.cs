using SquishIt.Framework;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Nancy.Web;
using Nancy;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Caching;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Nancy
{
    /// <summary>
    /// Extends the framework configuration with the System.Web-specific configuration data.
    /// </summary>
    public class DefaultConfiguration
    {
        /// <summary>
        /// Initialise with the web framework defaults
        /// </summary>
        public DefaultConfiguration(
            string virtualPathRoot,
            string physicalPathRoot,
            ICache cache,
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
            IContentCache bundleContentCache = null,
            IContentCache rawContentCache = null
            )
        {
            VirtualPathRoot = virtualPathRoot;
            PhysicalPathRoot = physicalPathRoot;

            UseHttpUtility(httpUtility ?? new HttpUtility());

            httpContext = httpContext ?? new HttpContext();
            UseHttpContext(httpContext);

            virtualPathUtility = virtualPathUtility ?? new VirtualPathUtilityWrapper();
            UseVirtualPathUtility(virtualPathUtility);

            pathTranslator = pathTranslator ?? new DefaultPathTranslator(new DefaultRootPathProvider());
            UsePathTranslator(pathTranslator);

            var machineConfigReader = new MachineConfigReader();
            DebugStatusReader = debugStatusReader ?? new DebugStatusReader(machineConfigReader, httpContext);

            UseTempPathProvider(tempPathProvider ?? new TempPathProvider());
            FileSystemResolver = fileSystemResolver ?? new FileSystemResolver();
            HttpResolver = httpResolver ?? new HttpResolver(DefaultTempPathProvider());
            RootEmbeddedResourceResolver = rootEmbeddedResourceResolver ?? new RootEmbeddedResourceResolver(DefaultTempPathProvider());
            StandardEmbeddedResourceResolver = standardEmbeddedResourceResolver ?? new StandardEmbeddedResourceResolver(DefaultTempPathProvider());

            bundleContentCache = bundleContentCache ?? new BundleContentCache(cache);
            RawContentCache = rawContentCache ?? new RawContentCache(cache);
        }
    }
}
