using SquishIt.Framework;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.AspNet.Web;
using SquishIt.Framework.Resolvers;

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
            IFileResolver standardEmbeddedResourceResolver = null
            )
        {
            UseHttpUtility(httpUtility ?? new HttpUtility());

            httpContext = httpContext ?? new HttpContext(System.Web.HttpContext.Current);
            UseHttpContext(httpContext);

            virtualPathUtility = virtualPathUtility ?? new VirtualPathUtilityWrapper();
            UseVirtualPathUtility(virtualPathUtility);

            virtualPathRoot = virtualPathRoot ?? System.Web.HttpRuntime.AppDomainAppVirtualPath;
            physicalPathRoot = physicalPathRoot ?? System.Web.HttpRuntime.AppDomainAppPath;

            pathTranslator = pathTranslator ?? new DefaultPathTranslator(virtualPathRoot, physicalPathRoot, httpContext, virtualPathUtility);
            UsePathTranslator(pathTranslator);

            var machineConfigReader = new MachineConfigReader();
            DebugStatusReader = debugStatusReader ?? new DebugStatusReader(machineConfigReader, new HttpContext(System.Web.HttpContext.Current));

            UseTempPathProvider(tempPathProvider ?? new TempPathProvider());
            FileSystemResolver = fileSystemResolver ?? new FileSystemResolver();
            HttpResolver = httpResolver ?? new HttpResolver(DefaultTempPathProvider());
            RootEmbeddedResourceResolver = rootEmbeddedResourceResolver ?? new RootEmbeddedResourceResolver(DefaultTempPathProvider());
            StandardEmbeddedResourceResolver = standardEmbeddedResourceResolver ?? new StandardEmbeddedResourceResolver(DefaultTempPathProvider());
        }
    }
}