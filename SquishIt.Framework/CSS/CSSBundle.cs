using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using SquishIt.Framework.Base;
using SquishIt.Framework.Caches;
using SquishIt.Framework.Minifiers;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Files;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;
using SquishIt.Framework.Invalidation;
using SquishIt.Framework.Renderers;

namespace SquishIt.Framework.CSS
{
    /// <summary>
    /// CSS Bundle implementation.
    /// </summary>
    public class CSSBundle : BundleBase<CSSBundle>
    {
        static readonly Regex IMPORT_PATTERN = new Regex(@"@import +url\(([""']){0,1}(.*?)\1{0,1}\);", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        const string CSS_TEMPLATE = "<link rel=\"stylesheet\" type=\"text/css\" {0}href=\"{1}\" />";
        const string CACHE_PREFIX = "css";
        const string TAG_FORMAT = "<style type=\"text/css\">{0}</style>";
        readonly IHttpUtility httpUtility;

        bool ShouldImport { get; set; }
        bool ShouldAppendHashForAssets { get; set; }


        protected override string Template
        {
            get { return CSS_TEMPLATE; }
        }

        protected override string CachePrefix
        {
            get { return CACHE_PREFIX; }
        }

        protected override IMinifier<CSSBundle> DefaultMinifier { get; }

        protected override IEnumerable<string> allowedFileExtensions
        {
            get { return bundleState.AllowedExtensions.Union(Bundle.AllowedGlobalExtensions.Union(Bundle.AllowedStyleExtensions)); }
        }

        protected override IEnumerable<string> disallowedFileExtensions
        {
            get { return Bundle.AllowedScriptExtensions; }
        }

        protected override string defaultFileExtension
        {
            get { return ".CSS"; }
        }

        protected override string tagFormat
        {
            get { return bundleState.Typeless ? TAG_FORMAT.Replace(" type=\"text/css\"", "") : TAG_FORMAT; }
        }

        //public CSSBundle(IDebugStatusReader debugStatusReader)
        //    : this(debugStatusReader, new FileWriterFactory(Configuration.Instance.DefaultRetryableFileOpener(), 5), new FileReaderFactory(Configuration.Instance.DefaultRetryableFileOpener(), 5), new DirectoryWrapper(), Configuration.Instance.DefaultHasher(), Configuration.Instance.BundleCache, Configuration.Instance.RawContentCache, Configuration.Instance.DefaultHttpUtility(), Configuration.Instance.DefaultOutputBaseHref(), Configuration.Instance.DefaultPathTranslator(),
        //          Configuration.Instance.FileSystemResolver, Configuration.Instance.HttpResolver, Configuration.Instance.RootEmbeddedResourceResolver, Configuration.Instance.StandardEmbeddedResourceResolver, Configuration.Instance.VirtualPathRoot)
        //{
        //}

        public CSSBundle(IDebugStatusReader debugStatusReader, IFileWriterFactory fileWriterFactory, IFileReaderFactory fileReaderFactory, IDirectoryWrapper directoryWrapper, IHasher hasher, IContentCache bundleCache, IContentCache rawContentCache, IHttpUtility httpUtility, string baseOutputHref, IPathTranslator pathTranslator,
            IResourceResolver resourceResolver, IRenderer releaseRenderer, Func<bool> debugPredicate, ICacheInvalidationStrategy cacheInvalidationStrategy, IFilePathMutexProvider filePathMutexProvider, ITrustLevel trustLevel, IMinifier<CSSBundle> cssMinifier, string hashKeyName, string virtualPathRoot)
            : base(fileWriterFactory, fileReaderFactory, debugStatusReader, directoryWrapper, hasher, bundleCache, rawContentCache, baseOutputHref, pathTranslator, resourceResolver, releaseRenderer, debugPredicate, cacheInvalidationStrategy, filePathMutexProvider, trustLevel, hashKeyName, virtualPathRoot)
        {
            this.httpUtility = httpUtility;
            DefaultMinifier = cssMinifier;

        }

        string ProcessImport(string file, string outputFile, string css)
        {
            //https://github.com/jetheredge/SquishIt/issues/215
            //if sourcePath is used below and doesn't start with a /, the path is not resolved relative to site root but relative to current folder
            var sourcePath = "/" + (PathTranslator.ResolveFileSystemPathToAppRelative(Path.GetDirectoryName(file)) + "/").TrimStart("/");

            return IMPORT_PATTERN.Replace(css, match =>
            {
                var importPath = match.Groups[2].Value;
                var import = importPath.StartsWith("/")
                    ? PathTranslator.ResolveAppRelativePathToFileSystem(importPath)
                    : PathTranslator.ResolveAppRelativePathToFileSystem(sourcePath + importPath);
                bundleState.DependentFiles.Add(import);

                var processCssFile = ProcessCssFile(import, outputFile, import, true);

                return processCssFile;
            });
        }

        /// <summary>
        /// Configure bundle to process CSS imports.
        /// </summary>
        public CSSBundle ProcessImports()
        {
            ShouldImport = true;
            return this;
        }

        /// <summary>
        /// Configure bundle to append versioning hash to asset reference URLs.
        /// </summary>
        public CSSBundle AppendHashForAssets()
        {
            ShouldAppendHashForAssets = true;
            return this;
        }

        protected override void AggregateContent(List<Asset> assets, StringBuilder sb, string outputFile)
        {
            assets.SelectMany(a => a.IsArbitrary
                                       ? new[] { PreprocessArbitrary(a) }.AsEnumerable()
                                       : GetFilenamesForSingleAsset(a).Select(f => ProcessFile(f, outputFile, a)))
                .ToList()
                .Distinct()
                .Aggregate(sb, (b, s) =>
                                   {
                                       b.Append(s);
                                       return b;
                                   });
        }

        protected override string ProcessFile(string file, string outputFile, Asset originalAsset)
        {
            var sourcePath = file;
            var pathForRewriter = originalAsset.IsEmbeddedResource ? PathTranslator.ResolveAppRelativePathToFileSystem(originalAsset.LocalPath) : file;
            return MinifyIfNeeded(ProcessCssFile(sourcePath, outputFile, pathForRewriter), originalAsset.Minify);
        }

        string ProcessCssFile(string file, string outputFile, string fileForCssRewriter, bool asImport = false)
        {
            var preprocessors = FindPreprocessors(file);

            var css = preprocessors.NullSafeAny() 
                ? PreprocessFile(file, preprocessors) 
                : ReadFile(file);

            if(ShouldImport)
            {
                css = ProcessImport(file, outputFile, css);
            }

            ICSSAssetsFileHasher fileHasher = null;

            if(ShouldAppendHashForAssets)
            {
                var fileResolver = new FileSystemResolver();
                fileHasher = new CSSAssetsFileHasher(bundleState.HashKeyName, fileResolver, hasher, PathTranslator, httpUtility);
            }

            return CSSPathRewriter.RewriteCssPaths(outputFile, fileForCssRewriter, css, fileHasher, PathTranslator, asImport: asImport);
        }
    }
}