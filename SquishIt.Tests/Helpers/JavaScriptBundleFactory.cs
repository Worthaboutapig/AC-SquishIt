using System;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Invalidation;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Renderers;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Stubs;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Tests.Helpers
{
    public class JavaScriptBundleFactory
    {
        private IDebugStatusReader _debugStatusReader = new StubDebugStatusReader();
        private IFileWriterFactory _fileWriterFactory = new StubFileWriterFactory();
        private IFileReaderFactory _fileReaderFactory = new StubFileReaderFactory();
        private IDirectoryWrapper _directoryWrapper = new StubDirectoryWrapper();
        private IHasher _hasher = new StubHasher("hash");
        private readonly IContentCache _contentCache = new StubContentCache();
        private readonly IContentCache _rawContentCache = new StubContentCache();

        private readonly string _baseOutputHref;
        private readonly IPathTranslator _pathTranslator;
        private readonly IResourceResolver _resourceResolver;
        private readonly IRenderer _releaseRenderer;
        private readonly Func<bool> _debugPredicate;
        private readonly ICacheInvalidationStrategy _cacheInvalidationStrategy;
        private readonly IMinifier<JavaScriptBundle> _javascriptMinifier;
        private readonly string _hashKeyName;
        private readonly string _virtualPathRoot;
        private readonly IFilePathMutexProvider _filePathMutexProvider;
        private readonly ITrustLevel _trustLevel;

        public JavaScriptBundleFactory(string baseOutputHref,
                                       IPathTranslator pathTranslator,
                                       IResourceResolver resourceResolver,
                                       IRenderer releaseRenderer,
                                       Func<bool> debugPredicate,
                                       ICacheInvalidationStrategy cacheInvalidationStrategy,
                                       IFilePathMutexProvider filePathMutexProvider,
                                       ITrustLevel trustLevel,
                                       IMinifier<JavaScriptBundle> javascriptMinifier,
                                       string hashKeyName,
                                       string virtualPathRoot)
        {
            _baseOutputHref = baseOutputHref;
            _pathTranslator = pathTranslator;
            _resourceResolver = resourceResolver;
            _releaseRenderer = releaseRenderer;
            _debugPredicate = debugPredicate;
            _cacheInvalidationStrategy = cacheInvalidationStrategy;
            _filePathMutexProvider = filePathMutexProvider;
            _trustLevel = trustLevel;
            _javascriptMinifier = javascriptMinifier;
            _hashKeyName = hashKeyName;
            _virtualPathRoot = virtualPathRoot;
        }

        public StubFileReaderFactory FileReaderFactory { get { return _fileReaderFactory as StubFileReaderFactory; } }
        public StubFileWriterFactory FileWriterFactory { get { return _fileWriterFactory as StubFileWriterFactory; } }

        public JavaScriptBundleFactory WithDebuggingEnabled(bool enabled)
        {
            _debugStatusReader = new StubDebugStatusReader(enabled);
            return this;
        }

        public JavaScriptBundleFactory WithFileWriterFactory(IFileWriterFactory fileWriterFactory)
        {
            _fileWriterFactory = fileWriterFactory;
            return this;
        }

        public JavaScriptBundleFactory WithFileReaderFactory(IFileReaderFactory fileReaderFactory)
        {
            _fileReaderFactory = fileReaderFactory;
            return this;
        }

        public JavaScriptBundleFactory WithCurrentDirectoryWrapper(IDirectoryWrapper directoryWrapper)
        {
            _directoryWrapper = directoryWrapper;
            return this;
        }

        public JavaScriptBundleFactory WithHasher(IHasher hasher)
        {
            _hasher = hasher;
            return this;
        }

        public JavaScriptBundle Create()
        {
            return new JavaScriptBundle(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _contentCache, _rawContentCache, _baseOutputHref, _pathTranslator, _resourceResolver, _releaseRenderer, _debugPredicate, _cacheInvalidationStrategy, _filePathMutexProvider, _trustLevel, _javascriptMinifier, _hashKeyName, _virtualPathRoot);
        }

        public JavaScriptBundleFactory WithContents(string css)
        {
            // Todo: Fix this.
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}