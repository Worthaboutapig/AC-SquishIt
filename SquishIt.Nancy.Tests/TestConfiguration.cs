using SquishIt.Framework;
using SquishIt.Framework.Caching;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Web;
using SquishIt.Tests.Stubs;

namespace SquishIt.Nancy.Tests
{
    internal class TestConfiguration
    {
        private static readonly IHttpRequest HttpRequest = new StubHttpRequest();
        private static readonly IHttpContext HttpContext = new StubHttpContext(httpRequest: HttpRequest);
        private static readonly IFileResolver HttpResolver = new StubHttpResolver();
        private static readonly ICache Cache = new MemoryCache();

        private static readonly DefaultConfiguration DefaultConfiguration = new DefaultConfiguration("/testVirtualPathRoot", "B:\\", httpContext: HttpContext, httpResolver: HttpResolver, cache: Cache);

        static TestConfiguration()
        {
            Configuration.Instance = Default;
        }

        public static DefaultConfiguration Default
        {
            get
            {
                return DefaultConfiguration;
            }
        }
    }
}
