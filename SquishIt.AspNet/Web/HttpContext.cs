using System;
using SquishIt.Framework.Web;
using System.Web;

namespace SquishIt.AspNet.Web
{
    /// <summary>
    /// Provides access to an <see cref="System.Web.HttpContext.Current"/>.
    /// </summary>
    public class HttpContext : IHttpContext
    {
        private readonly HttpContextWrapper _httpContextWrapper;
        private readonly IHttpRequest _httpRequest;
        private readonly IServer _server;

        /// <summary>
        /// Abstracts a given <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext"></param>
        public HttpContext(System.Web.HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException("httpContext");

            _httpContextWrapper = new HttpContextWrapper(httpContext);
            _httpRequest = new HttpRequest(_httpContextWrapper.Request);
            _server = new Server(_httpContextWrapper.Server);
        }


        /// <summary>
        /// The current HTTP request.
        /// </summary>
        public IHttpRequest Request { get { return _httpRequest; } }

        /// <summary>
        /// The server.
        /// </summary>
        public IServer Server { get { return _server; } }

        /// <summary>
        /// Indicates whether the current HTTP request is in debug mode.
        /// </summary>
        /// <returns>
        /// <c>True</c> if the request is in debug mode, <c>false</c> otherwise.
        /// </returns>
        public bool IsDebuggingEnabled { get { return _httpContextWrapper.IsDebuggingEnabled; } }
    }
}