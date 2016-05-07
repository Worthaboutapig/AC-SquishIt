using NUnit.Framework;

namespace SquishIt.AspNet.Tests
{
	using System;
	using SquishIt.Tests.Helpers;

	[TestFixture]
    public class CoffeeScriptTests : SquishIt.Tests.CoffeeScriptTests
    {
        public CoffeeScriptTests() : base(GetJavaScriptBundleFactory())
        {
		}

	    private static Func<JavaScriptBundleFactory> GetJavaScriptBundleFactory()
	    {
			TestConfiguration.Default.DefaultOutputBaseHref(),
            TestConfiguration.Default.DefaultPathTranslator(),
            TestConfiguration.Default.FileSystemResolver,
            TestConfiguration.Default.HttpResolver,
            TestConfiguration.Default.RootEmbeddedResourceResolver,
            TestConfiguration.Default.StandardEmbeddedResourceResolver,
            TestConfiguration.Default.VirtualPathRoot, TODO, TODO

				string baseOutputHref, IPathTranslator pathTranslator, IResourceResolver resourceResolver, IRenderer releaseRenderer, Func<bool> debugPredicate, ICacheInvalidationStrategy cacheInvalidationStrategy, IMinifier<JavaScriptBundle> javascriptMinifier, string hashKeyName, string virtualPathRoot
			Func<JavaScriptBundleFactory> javaScriptBundleFactory = () => new JavaScriptBundleFactory(_baseOutputHref, _pathTranslator, _resourceResolver, _releaseRenderer, _debugPredicate, _cacheInvalidationStrategy, _javascriptMinifier, _hashKeyName, _virtualPathRoot);

		    return javaScriptBundleFactory;
	    }
	}

}