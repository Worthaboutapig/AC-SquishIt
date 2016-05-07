using SquishIt.Framework;
using SquishIt.Framework.Caching;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Web;
using SquishIt.Tests.Web;

namespace SquishIt.Nancy.Tests
{
    internal class TestConfiguration
    {
        private static readonly IHttpRequest HttpRequest = new HttpRequestStub();
        private static readonly IHttpContext HttpContext = new HttpContextStub(httpRequest: HttpRequest);
        private static readonly IFileResolver HttpResolver = new HttpResolverStub();
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
