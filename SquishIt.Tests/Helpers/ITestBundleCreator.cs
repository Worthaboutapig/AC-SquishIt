using System;
using SquishIt.Framework;

namespace SquishIt.Tests.Helpers
{
    /// <summary>
    /// Adds methods for getting bundle factories.
    /// </summary>
    public interface ITestBundleCreator : IBundleCreator
    {
        /// <summary>
        /// Creates a <see cref="JavaScriptBundleFactory"/> when invoked.
        /// </summary>
        Func<JavaScriptBundleFactory> JavaScriptBundleFactoryCreator { get; }

        /// <summary>
        /// Creates a <see cref="CssBundleFactoryCreator"/> when invoked.
        /// </summary>
        Func<CssBundleFactory> CssBundleFactoryCreator { get; }
    }
}
