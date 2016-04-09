using System;

namespace SquishIt.Tests.Helpers
{
    using Framework.Web;

    public class HttpContextScope : IDisposable
    {
        private IHttpContext _httpContext;

        public HttpContextScope(IHttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public void Dispose()
        {
	        _httpContext = null;
        }
    }
}
