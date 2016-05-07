using System;
using SquishIt.Framework;
using SquishIt.Framework.Invalidation;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Renderers;
using SquishIt.Framework.Caches;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests.Helpers
{
    public class CssBundleFactory
    {
        private IDebugStatusReader _debugStatusReader = new StubDebugStatusReader();
        private IFileWriterFactory _fileWriterFactory = new StubFileWriterFactory();
        private IFileReaderFactory _fileReaderFactory = new StubFileReaderFactory();
        private IDirectoryWrapper _directoryWrapper = new StubDirectoryWrapper();
        private IHasher _hasher = new StubHasher("hash");
        private readonly IContentCache _contentCache = new StubContentCache();
        private readonly IContentCache _rawContentCache = new StubContentCache();
        private readonly IHttpUtility _httpUtility;
        private readonly string _baseOutputHref;
        private readonly IPathTranslator _pathTranslator;
        private readonly IResourceResolver _resourceResolver;
        private readonly IRenderer _releaseRenderer;
        private readonly Func<bool> _debugPredicate;
        private readonly ICacheInvalidationStrategy _cacheInvalidationStrategy;
        private readonly IFilePathMutexProvider _filePathMutexProvider;
        private readonly ITrustLevel _trustLevel;
        private readonly IMinifier<CSSBundle> _cssMinifier;
        private readonly string _hashKeyName;
        private readonly string _virtualPathRoot;

        public StubFileReaderFactory FileReaderFactory { get { return _fileReaderFactory as StubFileReaderFactory; } }
        public StubFileWriterFactory FileWriterFactory { get { return _fileWriterFactory as StubFileWriterFactory; } }

        public CssBundleFactory(IHttpUtility httpUtility,
                                string baseOutputHref,
                                IPathTranslator pathTranslator,
                                IResourceResolver resourceResolver,
                                IRenderer releaseRenderer,
                                Func<bool> debugPredicate,
                                ICacheInvalidationStrategy cacheInvalidationStrategy,
                                IFilePathMutexProvider filePathMutexProvider,
                                ITrustLevel trustLevel,
                                IMinifier<CSSBundle> cssMinifier,
                                string hashKeyName,
                                string virtualPathRoot)
        {
            _httpUtility = httpUtility;
            _baseOutputHref = baseOutputHref;
            _pathTranslator = pathTranslator;
            _resourceResolver = resourceResolver;
            _releaseRenderer = releaseRenderer;
            _debugPredicate = debugPredicate;
            _cacheInvalidationStrategy = cacheInvalidationStrategy;
            _filePathMutexProvider = filePathMutexProvider;
            _trustLevel = trustLevel;
            _cssMinifier = cssMinifier;
            _hashKeyName = hashKeyName;
            _virtualPathRoot = virtualPathRoot;
        }

        public CssBundleFactory WithDebuggingEnabled(bool enabled)
        {
            _debugStatusReader = new StubDebugStatusReader(enabled);
            return this;
        }

        public CssBundleFactory WithFileWriterFactory(IFileWriterFactory fileWriterFactory)
        {
            _fileWriterFactory = fileWriterFactory;
            return this;
        }

        public CssBundleFactory WithFileReaderFactory(IFileReaderFactory fileReaderFactory)
        {
            _fileReaderFactory = fileReaderFactory;
            return this;
        }

        public CssBundleFactory WithCurrentDirectoryWrapper(IDirectoryWrapper directoryWrapper)
        {
            _directoryWrapper = directoryWrapper;
            return this;
        }

        public CssBundleFactory WithHasher(IHasher hasher)
        {
            _hasher = hasher;
            return this;
        }

        public CSSBundle Create()
        {
            var cssBundle = new CSSBundle(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _contentCache, _rawContentCache, _httpUtility, _baseOutputHref, _pathTranslator, _resourceResolver, _releaseRenderer, _debugPredicate, _cacheInvalidationStrategy, _filePathMutexProvider, _trustLevel, _cssMinifier, _hashKeyName, _virtualPathRoot);
            return cssBundle;
        }

        public CssBundleFactory WithContents(string css)
        {
            // Todo: Fix.
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}