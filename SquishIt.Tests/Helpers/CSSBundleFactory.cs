using System;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Tests.Stubs;

namespace SquishIt.Tests.Helpers
{
    public class BundleFactory
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

        public StubFileReaderFactory FileReaderFactory { get { return _fileReaderFactory as StubFileReaderFactory; } }
        public StubFileWriterFactory FileWriterFactory { get { return _fileWriterFactory as StubFileWriterFactory; } }

        public BundleFactory(Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> bundleCreatorFunc)
        {
            _bundleCreatorFunc = bundleCreatorFunc;
        }

        public BundleFactory WithDebuggingEnabled(bool enabled)
        {
            _debugStatusReader = new StubDebugStatusReader(enabled);
            return this;
        }

        public BundleFactory WithFileWriterFactory(IFileWriterFactory fileWriterFactory)
        {
            _fileWriterFactory = fileWriterFactory;
            return this;
        }

        public BundleFactory WithFileReaderFactory(IFileReaderFactory fileReaderFactory)
        {
            _fileReaderFactory = fileReaderFactory;
            return this;
        }

        public BundleFactory WithCurrentDirectoryWrapper(IDirectoryWrapper directoryWrapper)
        {
            _directoryWrapper = directoryWrapper;
            return this;
        }

        public BundleFactory WithHasher(IHasher hasher)
        {
            _hasher = hasher;
            return this;
        }

        public BundleFactory WithTrustLevel(ITrustLevel trustLevel)
        {
            _trustLevel = trustLevel;
            return this;
        }

        public CSSBundle Create()
        {
            var bundleCreator = _bundleCreatorFunc(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _bundleContentCache, _rawContentCache, _trustLevel);
            return bundleCreator.GetCssBundle();
        }

        public BundleFactory WithContents(string css)
        {
            // Todo: Fix.
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }

    public class CssBundleFactory
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

        public StubFileReaderFactory FileReaderFactory { get { return _fileReaderFactory as StubFileReaderFactory; } }
        public StubFileWriterFactory FileWriterFactory { get { return _fileWriterFactory as StubFileWriterFactory; } }

        public CssBundleFactory(Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> bundleCreatorFunc)
        {
            _bundleCreatorFunc = bundleCreatorFunc;
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

        public CssBundleFactory WithTrustLevel(ITrustLevel trustLevel)
        {
            _trustLevel = trustLevel;
            return this;
        }

        public CSSBundle Create()
        {
            var bundleCreator = _bundleCreatorFunc(_debugStatusReader, _fileWriterFactory, _fileReaderFactory, _directoryWrapper, _hasher, _bundleContentCache, _rawContentCache, _trustLevel);
            return bundleCreator.GetCssBundle();
        }

        public CssBundleFactory WithContents(string css)
        {
            // Todo: Fix.
            (_fileReaderFactory as StubFileReaderFactory).SetContents(css);
            return this;
        }
    }
}