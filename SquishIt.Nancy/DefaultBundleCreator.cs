using System;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Caching;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.Invalidation;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Renderers;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Nancy.Web;
using HttpUtility = Nancy.Helpers.HttpUtility;

namespace SquishIt.Nancy
{
    /// <summary>
    /// Extends the framework configuration with the System.Web-specific configuration data.
    /// </summary>
    public class DefaultBundleCreator : IBundleCreator
    {
        /// <summary>
        /// Initialise with the web framework defaults
        /// </summary>
        public DefaultBundleCreator(Type cssMinifierType = null, Type javascriptMinifierType = null, IPathTranslator pathTranslator = null, IHttpUtility httpUtility = null, IHttpContext httpContext = null, IVirtualPathUtility virtualPathUtility = null, IDebugStatusReader debugStatusReader = null, IFileWriterFactory fileWriterFactory = null, IFileReaderFactory fileReaderFactory = null, ITempPathProvider tempPathProvider = null, IFolderResolver fileSystemResolver = null, IFileResolver httpResolver = null, IFileResolver rootEmbeddedResourceResolver = null, IFileResolver standardEmbeddedResourceResolver = null, ICache cache = null, IContentCache bundleContentCache = null, IContentCache rawContentCache = null, string applicationPhysicalPath = null, IMachineConfigReader machineConfigReader = null, ICacheInvalidationStrategy cacheInvalidationStrategy = null, IRenderer releaseRenderer = null, IDirectoryWrapper directoryWrapper = null, IHasher hasher = null, string baseOutputHref = null, ITrustLevel trustLevel = null, IFilePathMutexProvider filePathMutexProvider = null, IResourceResolver resourceResolver = null, Func<bool> debugPredicate = null, IRetryableFileOpener retryableFileOpener = null, int numberOfRetries = 5, string virtualPath = null, string hashKeyName = "r")
        {
            _virtualPath = virtualPath ?? "";
            virtualPathUtility = virtualPathUtility ?? new VirtualPathUtilityWrapper();
            HttpUtility = httpUtility ?? new HttpUtility();
            httpContext = httpContext ?? new HttpContext();

            if (pathTranslator == null)
            {
                applicationPhysicalPath = applicationPhysicalPath ?? pathResolver.GetRootPath();
                PathTranslator = new DefaultPathTranslator(_virtualPath, applicationPhysicalPath, httpContext, virtualPathUtility);
            }
            else
            {
                PathTranslator = pathTranslator;
            }

            machineConfigReader = machineConfigReader ?? new MachineConfigReader();
            _trustLevel = trustLevel ?? new TrustLevel();
            _debugStatusReader = debugStatusReader ?? new DebugStatusReader(machineConfigReader, httpContext, _trustLevel);

            retryableFileOpener = retryableFileOpener ?? new RetryableFileOpener();
            _fileWriterFactory = fileWriterFactory ?? new FileWriterFactory(retryableFileOpener, numberOfRetries);
            _fileReaderFactory = fileReaderFactory ?? new FileReaderFactory(retryableFileOpener, numberOfRetries);
            _directoryWrapper = directoryWrapper ?? new DirectoryWrapper();
            _hasher = hasher ?? new Hasher(retryableFileOpener);
            _baseOutputHref = baseOutputHref ?? "";
            _cacheInvalidationStrategy = cacheInvalidationStrategy ?? new DefaultCacheInvalidationStrategy();
            tempPathProvider = tempPathProvider ?? new TempPathProvider();
            if (resourceResolver == null)
            {
                fileSystemResolver = fileSystemResolver ?? new FileSystemResolver();
                httpResolver = httpResolver ?? new HttpResolver(tempPathProvider);
                rootEmbeddedResourceResolver = rootEmbeddedResourceResolver ?? new RootEmbeddedResourceResolver(tempPathProvider);
                standardEmbeddedResourceResolver = standardEmbeddedResourceResolver ?? new StandardEmbeddedResourceResolver(tempPathProvider);
                _resourceResolver = new DefaultResourceResolver(fileSystemResolver, httpResolver, rootEmbeddedResourceResolver, standardEmbeddedResourceResolver);
            }
            else
            {
                _resourceResolver = resourceResolver;
            }

            cache = cache ?? new MemoryCache();
            _bundleContentCache = bundleContentCache ?? new BundleContentCache(cache);
            _rawContentCache = rawContentCache ?? new RawContentCache(cache);
            _filePathMutexProvider = filePathMutexProvider ?? new FilePathMutexProvider(_hasher, PathTranslator);
            _hashKeyName = hashKeyName;
            _debugPredicate = debugPredicate;
            _releaseRenderer = releaseRenderer ?? new FileRenderer(_fileWriterFactory);

            javascriptMinifierType = javascriptMinifierType ?? typeof(Framework.Minifiers.JavaScript.NullMinifier);
            if (!typeof(IMinifier<JavaScriptBundle>).IsAssignableFrom(javascriptMinifierType))
            {
                throw new InvalidCastException(string.Format("Type '{0}' must implement '{1}' to be used for Javascript minification.", javascriptMinifierType, typeof(IMinifier<JavaScriptBundle>)));
            }
            _javascriptMinifier = (IMinifier<JavaScriptBundle>) Activator.CreateInstance(javascriptMinifierType, true);

            cssMinifierType = cssMinifierType ?? typeof(Framework.Minifiers.CSS.NullMinifier);
            if (!typeof(IMinifier<CSSBundle>).IsAssignableFrom(cssMinifierType))
            {
                throw new InvalidCastException(string.Format("Type '{0}' must implement '{1}' to be used for Javascript minification.", cssMinifierType, typeof(IMinifier<CSSBundle>)));
            }
            _cssMinifier = (IMinifier<CSSBundle>)Activator.CreateInstance(cssMinifierType, true);
        }

        public IHttpUtility HttpUtility { get; private set; }
        public IPathTranslator PathTranslator { get; private set; }

        private readonly string _virtualPath;
        private readonly IDebugStatusReader _debugStatusReader;
        private readonly ITrustLevel _trustLevel;
        //private readonly ITempPathProvider _tempPathProvider;
        //private readonly IFolderResolver _fileSystemResolver;
        private readonly IFileWriterFactory _fileWriterFactory;
        private readonly IFileReaderFactory _fileReaderFactory;
        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IHasher _hasher;
        private readonly string _baseOutputHref;
        private readonly ICacheInvalidationStrategy _cacheInvalidationStrategy;
        private readonly IContentCache _bundleContentCache;
        private readonly IContentCache _rawContentCache;
        private readonly IResourceResolver _resourceResolver;
        private readonly IFilePathMutexProvider _filePathMutexProvider;
        private readonly string _hashKeyName;
        private readonly Func<bool> _debugPredicate;
        private readonly IRenderer _releaseRenderer;
        private readonly IMinifier<JavaScriptBundle> _javascriptMinifier;
        private readonly IMinifier<CSSBundle> _cssMinifier;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public JavaScriptBundle GetJavaScriptBundle()
        {
            var javascriptBundle = new JavaScriptBundle(_virtualPath, _debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _bundleContentCache,
                _rawContentCache, _baseOutputHref, PathTranslator, _resourceResolver, _releaseRenderer, _debugPredicate, _cacheInvalidationStrategy, _filePathMutexProvider,
                _trustLevel, _javascriptMinifier, _hashKeyName);

            return javascriptBundle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CSSBundle GetCssBundle()
        {
            var cssBundle = new CSSBundle(_virtualPath, _debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _bundleContentCache,
                _rawContentCache, HttpUtility, _baseOutputHref, PathTranslator, _resourceResolver, _releaseRenderer, _debugPredicate, _cacheInvalidationStrategy, _filePathMutexProvider,
                _trustLevel, _cssMinifier, _hashKeyName);

            return cssBundle;
        }
    }
}