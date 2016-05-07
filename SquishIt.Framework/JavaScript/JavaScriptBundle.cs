using System.Collections.Generic;
using System.Linq;
using System.Text;
using SquishIt.Framework.Base;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Files;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Utilities;

namespace SquishIt.Framework.JavaScript
{
    using System;
    using Invalidation;
    using Renderers;

    /// <summary>
    /// JavaScript bundle implementation.
    /// </summary>
    public class JavaScriptBundle : BundleBase<JavaScriptBundle>
    {
        const string JS_TEMPLATE = "<script type=\"text/javascript\" {0}src=\"{1}\" defer async></script>";
        const string TAG_FORMAT = "<script type=\"text/javascript\">{0}</script>";

        const string CACHE_PREFIX = "js";

        bool deferred;
        bool async;

        protected override IMinifier<JavaScriptBundle> DefaultMinifier { get; }

        protected override IEnumerable<string> allowedFileExtensions
        {
            get { return bundleState.AllowedExtensions.Union(Bundle.AllowedGlobalExtensions.Union(Bundle.AllowedScriptExtensions)); }
        }

        protected override IEnumerable<string> disallowedFileExtensions
        {
            get { return Bundle.AllowedStyleExtensions; }
        }

        protected override string defaultFileExtension
        {
            get { return ".JS"; }
        }

        protected override string tagFormat
        {
            get { return bundleState.Typeless ? TAG_FORMAT.Replace(" type=\"text/javascript\"", "") : TAG_FORMAT; }
        }

        //public JavaScriptBundle(IDebugStatusReader debugStatusReader)
        //    : this(debugStatusReader, new FileWriterFactory(Configuration.Instance.DefaultRetryableFileOpener(), 5), new FileReaderFactory(Configuration.Instance.DefaultRetryableFileOpener(), 5), new DirectoryWrapper(), Configuration.Instance.DefaultHasher(), Configuration.Instance.BundleCache, Configuration.Instance.RawContentCache, Configuration.Instance.DefaultOutputBaseHref(), Configuration.Instance.DefaultPathTranslator(),
        //    Configuration.Instance.FileSystemResolver, Configuration.Instance.HttpResolver, Configuration.Instance.RootEmbeddedResourceResolver, Configuration.Instance.StandardEmbeddedResourceResolver, Configuration.Instance.VirtualPathRoot)
        //{
        //}

        public JavaScriptBundle(IDebugStatusReader debugStatusReader, IFileWriterFactory fileWriterFactory, IFileReaderFactory fileReaderFactory, IDirectoryWrapper directoryWrapper, IHasher hasher,
            IContentCache bundleCache, IContentCache rawContentCache, string baseOutputHref, IPathTranslator pathTranslator,
            IResourceResolver resourceResolver, IRenderer releaseRenderer, Func<bool> debugPredicate, ICacheInvalidationStrategy cacheInvalidationStrategy, IFilePathMutexProvider filePathMutexProvider, ITrustLevel trustLevel, IMinifier<JavaScriptBundle> javsacriptMinifier, string hashKeyName, string virtualPathRoot) :
            base(fileWriterFactory, fileReaderFactory, debugStatusReader, directoryWrapper, hasher, bundleCache, rawContentCache, baseOutputHref, pathTranslator, resourceResolver, releaseRenderer, debugPredicate, cacheInvalidationStrategy, filePathMutexProvider, trustLevel, hashKeyName, virtualPathRoot)
        {
            DefaultMinifier = javsacriptMinifier;
        }

        protected override string Template
        {
            get
            {
                var val = bundleState.Typeless ? JS_TEMPLATE.Replace("type=\"text/javascript\" ", "") : JS_TEMPLATE;

                return deferred ? val.Replace(" async", "") : async ? val.Replace(" defer", "") : val.Replace(" defer", "").Replace(" async", "");
            }
        }

        protected override string CachePrefix
        {
            get { return CACHE_PREFIX; }
        }

        protected override string ProcessFile(string file, string outputFile, Asset originalAsset)
        {
            var preprocessors = FindPreprocessors(file);
            string content;
            if (preprocessors.NullSafeAny())
            {
                content = PreprocessFile(file, preprocessors);
            }
            else
            {
                content = ReadFile(file);
            }
            var minifyIfNeeded = MinifyIfNeeded(content, originalAsset.Minify);

            return minifyIfNeeded;
        }

        protected override void AggregateContent(List<Asset> assets, StringBuilder sb, string outputFile)
        {
            assets.SelectMany(a => a.IsArbitrary
                ? new[] {PreprocessArbitrary(a)}.AsEnumerable()
                : GetFilenamesForSingleAsset(a).Select(f => ProcessFile(f, outputFile, a)))
                  .ToList()
                  .Distinct()
                  .Aggregate(sb, (b, s) =>
                                 {
                                     b.Append(s);
                                     return b;
                                 });
        }

        const string MINIFIED_FILE_SEPARATOR = ";\n";

        protected override string AppendFileClosure(string content)
        {
            return content.TrimEnd(MINIFIED_FILE_SEPARATOR).TrimEnd(";") + MINIFIED_FILE_SEPARATOR;
        }

        /// <summary>
        /// Configure bundle to render with "deferred" attribute (script only).
        /// </summary>
        public JavaScriptBundle WithDeferredLoad()
        {
            deferred = true;
            async = false;
            return this;
        }

        /// <summary>
        /// Configure bundle to render with "async" attribute (script only).
        /// </summary>
        public JavaScriptBundle WithAsyncLoad()
        {
            async = true;
            deferred = false;
            return this;
        }
    }
}