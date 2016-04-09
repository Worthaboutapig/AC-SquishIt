using System.Diagnostics.Contracts;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Stubs;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Tests.Helpers
{
    internal class JavaScriptBundleFactory
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
        private readonly IFolderResolver _fileSystemResolver;
        private readonly IFileResolver _httpResolver;
        private readonly IFileResolver _rootEmbeddedResourceResolver;
        private readonly IFileResolver _standardEmbeddedResourceResolver;

        public JavaScriptBundleFactory(string baseOutputHref, IPathTranslator pathTranslator, IFolderResolver fileSystemResolver, IFileResolver httpResolver, IFileResolver rootEmbeddedResourceResolver, IFileResolver standardEmbeddedResourceResolver)
        {
            Contract.Requires(baseOutputHref != null);
            Contract.Requires(pathTranslator != null);
            Contract.Requires(fileSystemResolver != null);
            Contract.Requires(httpResolver != null);
            Contract.Requires(rootEmbeddedResourceResolver != null);
            Contract.Requires(standardEmbeddedResourceResolver != null);

            Contract.Ensures(_baseOutputHref != null);
            Contract.Ensures(_pathTranslator != null);
            Contract.Ensures(_fileSystemResolver != null);
            Contract.Ensures(_httpResolver != null);
            Contract.Ensures(_rootEmbeddedResourceResolver != null);
            Contract.Ensures(_standardEmbeddedResourceResolver != null);

            _baseOutputHref = baseOutputHref;
            _pathTranslator = pathTranslator;
            _fileSystemResolver = fileSystemResolver;
            _httpResolver = httpResolver;
            _rootEmbeddedResourceResolver = rootEmbeddedResourceResolver;
            _standardEmbeddedResourceResolver = standardEmbeddedResourceResolver;
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
            return new JavaScriptBundle(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _contentCache, _rawContentCache, _baseOutputHref, _pathTranslator, _fileSystemResolver, _httpResolver, _rootEmbeddedResourceResolver, _standardEmbeddedResourceResolver);
        }

        public JavaScriptBundleFactory WithContents(string css)
        {
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}