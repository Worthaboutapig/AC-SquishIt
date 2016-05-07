using SquishIt.Framework;
using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Web;
using SquishIt.Tests.Web;

namespace SquishIt.AspNet.Tests
{
    internal class TestConfiguration
    {
        private static readonly IHttpRequest HttpRequest = new HttpRequestStub();
        private static readonly IHttpContext HttpContext = new HttpContextStub(httpRequest: HttpRequest);
        private static readonly IFileResolver HttpResolver = new HttpResolverStub();

        private static readonly DefaultBundleCreator DefaultBundleCreator = new DefaultBundleCreator("/testVirtualPathRoot", "B:\\", httpContext: HttpContext, httpResolver: HttpResolver);

        static TestConfiguration()
        {
            Configuration.Instance = Default;
        }

        public static DefaultBundleCreator Default
        {
            get
            {
                return DefaultBundleCreator;
            }
        }
    }
}