using System;
using SquishIt.AspNet.Web;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Tests.Helpers;
using SquishIt.Tests.Stubs;

namespace SquishIt.AspNet.Tests
{
    /// <summary>
    /// 
    /// </summary>
    internal class TestBundleCreator : SquishIt.Tests.Helpers.TestBundleCreator
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TestBundleCreator(string sitePhysicalPath, string virtualPath = null, string applicationPhysicalPath = null)
        {
            var serverStub = new ServerStub(sitePhysicalPath, virtualPath, applicationPhysicalPath);
            var stubHttpRequest = new StubHttpRequest();
            _httpContext = new StubHttpContext(stubHttpRequest, serverStub);
            _httpResolver = new StubHttpResolver();

            HttpUtility = new HttpUtility();
            _virtualPathUtilityWrapper = new VirtualPathUtilityWrapper();
            PathTranslator = new DefaultPathTranslator(sitePhysicalPath, _httpContext, _virtualPathUtilityWrapper, applicationPhysicalPath);

            _bundleCreator = new DefaultBundleCreator(sitePhysicalPath: sitePhysicalPath, applicationPhysicalPath: applicationPhysicalPath, virtualPath: virtualPath, httpContext: _httpContext, httpResolver: _httpResolver, httpUtility: HttpUtility, virtualPathUtility: _virtualPathUtilityWrapper);

            JavaScriptBundleFactoryCreator = () => new JavaScriptBundleFactory(GetFactoryBundleCreator(sitePhysicalPath, virtualPath, applicationPhysicalPath));
            CssBundleFactoryCreator = () => new CssBundleFactory(GetFactoryBundleCreator(sitePhysicalPath, virtualPath, applicationPhysicalPath));
        }

        /// <summary>
        /// Creates a <see cref="JavaScriptBundleFactory"/> when invoked.
        /// </summary>
        public override Func<JavaScriptBundleFactory> JavaScriptBundleFactoryCreator { get; }

        /// <summary>
        /// Creates a <see cref="CssBundleFactory"/> when invoked.
        /// </summary>
        public override Func<CssBundleFactory> CssBundleFactoryCreator { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override JavaScriptBundle GetJavaScriptBundle()
        {
            return _bundleCreator.GetJavaScriptBundle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override CSSBundle GetCssBundle()
        {
            return _bundleCreator.GetCssBundle();
        }

        /// <summary>
        /// The HTTP utility.
        /// </summary>
        public IHttpUtility HttpUtility { get; private set; }

        /// <summary>
        /// The path translator.
        /// </summary>
        public IPathTranslator PathTranslator { get; private set; }

        private Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> GetFactoryBundleCreator(string applicationPhysicalPath, string virtualPath = null, string virtualSitePhysicalPath = null)
        {
            return (debugStatusReader, fileWriterFactory, fileReaderFactory, directoryWrapper, hasher, bundleContentCache, rawContentCache, trustLevel) =>
                   {
                       var bundleCreator = new DefaultBundleCreator(debugStatusReader: debugStatusReader, fileWriterFactory: fileWriterFactory, fileReaderFactory: fileReaderFactory, directoryWrapper: directoryWrapper, hasher: hasher, bundleContentCache: bundleContentCache, rawContentCache: rawContentCache,
                           sitePhysicalPath: applicationPhysicalPath, virtualPathUtility: _virtualPathUtilityWrapper, virtualPath: virtualPath, httpContext: _httpContext, httpResolver: _httpResolver, trustLevel: trustLevel);

                       return bundleCreator;
                   };
        }

        internal static TestBundleCreator Default = new TestBundleCreator(SitePhysicalPath);

        private readonly StubHttpContext _httpContext;
        private readonly StubHttpResolver _httpResolver;
        private readonly VirtualPathUtilityWrapper _virtualPathUtilityWrapper;
        private readonly IBundleCreator _bundleCreator;
    }
}