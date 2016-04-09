using System.Diagnostics.Contracts;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests.Helpers
{
    internal class CssBundleFactory
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
        private readonly FileSystemResolver _fileSystemResolver;
        private readonly HttpResolver _httpResolver;
        private readonly RootEmbeddedResourceResolver _rootEmbeddedResourceResolver;
        private readonly StandardEmbeddedResourceResolver _standardEmbeddedResourceResolver;

        public StubFileReaderFactory FileReaderFactory { get { return _fileReaderFactory as StubFileReaderFactory; } }
        public StubFileWriterFactory FileWriterFactory { get { return _fileWriterFactory as StubFileWriterFactory; } }

        public CssBundleFactory(IHttpUtility httpUtility, string baseOutputHref, IPathTranslator pathTranslator, FileSystemResolver fileSystemResolver, HttpResolver httpResolver, RootEmbeddedResourceResolver rootEmbeddedResourceResolver, StandardEmbeddedResourceResolver standardEmbeddedResourceResolver)
        {
            Contract.Requires(httpUtility != null);
            Contract.Requires(baseOutputHref != null);
            Contract.Requires(pathTranslator != null);
            Contract.Requires(fileSystemResolver != null);
            Contract.Requires(httpResolver != null);
            Contract.Requires(rootEmbeddedResourceResolver != null);
            Contract.Requires(standardEmbeddedResourceResolver != null);

            Contract.Ensures(_httpUtility != null);
            Contract.Ensures(_baseOutputHref != null);
            Contract.Ensures(_pathTranslator != null);
            Contract.Ensures(_fileSystemResolver != null);
            Contract.Ensures(_httpResolver != null);
            Contract.Ensures(_rootEmbeddedResourceResolver != null);
            Contract.Ensures(_standardEmbeddedResourceResolver != null);

            _httpUtility = httpUtility;
            _baseOutputHref = baseOutputHref;
            _pathTranslator = pathTranslator;
            _fileSystemResolver = fileSystemResolver;
            _httpResolver = httpResolver;
            _rootEmbeddedResourceResolver = rootEmbeddedResourceResolver;
            _standardEmbeddedResourceResolver = standardEmbeddedResourceResolver;
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
            return new CSSBundle(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _contentCache, _rawContentCache, _httpUtility, _baseOutputHref, _pathTranslator, _fileSystemResolver, _httpResolver, _rootEmbeddedResourceResolver, _standardEmbeddedResourceResolver);
        }

        public CssBundleFactory WithContents(string css)
        {
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}