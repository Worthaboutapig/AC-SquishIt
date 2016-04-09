using SquishIt.Framework.Web;

namespace SquishIt.Tests.Web
{
    public class HttpContextStub : IHttpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HttpContextStub(bool isDebuggingEnabled = false, IHttpRequest httpRequest = null, IServer server = null)
        {
            IsDebuggingEnabled = isDebuggingEnabled;
            Request = httpRequest ?? new HttpRequestStub();
            Server = server;
        }

        public bool IsDebuggingEnabled { get; set; }

        public IHttpRequest Request { get; set; }

        public IServer Server { get; set; }
    }
}
