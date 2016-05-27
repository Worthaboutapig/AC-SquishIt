using System;
using SquishIt.Framework;
using SquishIt.Framework.Caches;
using SquishIt.Framework.CSS;
using SquishIt.Framework.Files;
using SquishIt.Framework.JavaScript;
using SquishIt.Framework.Utilities;
using SquishIt.Framework.Web;

namespace SquishIt.Tests.Helpers
{
    public abstract class TestBundleCreator : ITestBundleCreator
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected TestBundleCreator() //IBundleCreator bundleCreator, Func<IDebugStatusReader, IFileWriterFactory, IFileReaderFactory, IDirectoryWrapper, IHasher, IContentCache, IContentCache, ITrustLevel, IBundleCreator> javaScriptFactoryBundleFactory, CssBundleFactory cssBundleFactory)
        {
//            _bundleCreator = bundleCreator;
//            JavaScriptBundleFactory = new JavaScriptBundleFactory(javaScriptFactoryBundleFactory);
//            CssBundleFactory = cssBundleFactory;

            //HttpUtility = bundleCreator.;
        }

        //private readonly IBundleCreator _bundleCreator;
        protected static readonly string SitePhysicalPath = Environment.CurrentDirectory;
        //public JavaScriptBundleFactory JavaScriptBundleFactory { get; }
        //public CssBundleFactory CssBundleFactory { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public JavaScriptBundle GetJavaScriptBundle()
        //{
        //    return _bundleCreator.GetJavaScriptBundle();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public CSSBundle GetCssBundle()
        //{
        //    return _bundleCreator.GetCssBundle();
        //}

        ///// <summary>
        ///// The HTTP utility.
        ///// </summary>
        //public IHttpUtility HttpUtility { get; private set; }

        ///// <summary>
        ///// The path translator.
        ///// </summary>
        //public IPathTranslator PathTranslator { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract JavaScriptBundle GetJavaScriptBundle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract CSSBundle GetCssBundle();
        public abstract JavaScriptBundleFactory JavaScriptBundleFactory { get; }
        public abstract CssBundleFactory CssBundleFactory { get; }
    }
}