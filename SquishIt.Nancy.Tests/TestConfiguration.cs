using SquishIt.Framework.Resolvers;
using SquishIt.Framework.Web;
using SquishIt.Nancy;
using SquishIt.Tests.Web;

namespace SquishIt.Nancy.Tests
{
    internal class TestConfiguration
    {
        private static IHttpRequest _httpRequest = new HttpRequestStub();
        private static IHttpContext _httpContext = new HttpContextStub(httpRequest: _httpRequest);
        private static IFileResolver _httpResolver = new HttpResolverStub();

        private static DefaultConfiguration _defaultConfiguration = new DefaultConfiguration("/testVirtualPathRoot", "B:\\", httpContext: _httpContext, httpResolver: _httpResolver);

        public static DefaultConfiguration Default
        {
            get
            {
                return _defaultConfiguration;
            }
        }
    }
}
