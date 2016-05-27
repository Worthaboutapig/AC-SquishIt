using System;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests.Helpers
{
    public class JavaScriptBundleFactory
    {
        private IDebugStatusReader _debugStatusReader = new StubDebugStatusReader();
        private IFileWriterFactory _fileWriterFactory = new StubFileWriterFactory();
        private IFileReaderFactory _fileReaderFactory = new StubFileReaderFactory();
        private IDirectoryWrapper _directoryWrapper = new StubDirectoryWrapper();
        private IHasher _hasher = new StubHasher("hash");
        private ITrustLevel _trustLevel = new TrustLevel();
        private readonly IContentCache _bundleContentCache = new StubContentCache();
        private readonly IContentCache _rawContentCache = new StubContentCache();

        private readonly Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> _bundleCreatorFunc;

        public JavaScriptBundleFactory(Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> bundleCreatorFunc)
        {
            _bundleCreatorFunc = bundleCreatorFunc;
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

        public JavaScriptBundleFactory WithTrustLevel(ITrustLevel trustLevel)
        {
            _trustLevel = trustLevel;
            return this;
        }

        public JavaScriptBundle Create()
        {
            var bundleCreator = _bundleCreatorFunc(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _bundleContentCache, _rawContentCache, _trustLevel);
            return bundleCreator.GetJavaScriptBundle();
        }

        public JavaScriptBundleFactory WithContents(string css)
        {
            // Todo: Fix this.
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}