using System;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace SquishIt.Tests.Helpers
{
    public abstract class TestBundleCreator : ITestBundleCreator
    {
        protected static readonly string SitePhysicalPath = Environment.CurrentDirectory;

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

        /// <summary>
        /// Creates a <see cref="JavaScriptBundleFactory"/> when invoked.
        /// </summary>
        public abstract Func<JavaScriptBundleFactory> JavaScriptBundleFactoryCreator { get; }

        /// <summary>
        /// Creates a <see cref="CssBundleFactory"/> when invoked.
        /// </summary>
        public abstract Func<CssBundleFactory> CssBundleFactoryCreator { get; }
    }
}