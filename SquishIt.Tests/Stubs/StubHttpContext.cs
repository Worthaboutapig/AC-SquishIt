using System;
using SquishIt.Framework.Web;

namespace SquishIt.Tests.Stubs
{
    public class StubHttpContext : IHttpContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public StubHttpContext(IHttpRequest httpRequest, IServer server, bool isDebuggingEnabled = false)
        {
            if (httpRequest == null) throw new ArgumentNullException("httpRequest");
            if (server == null) throw new ArgumentNullException("server");

            Request = httpRequest;
            Server = server;
            IsDebuggingEnabled = isDebuggingEnabled;
        }

        public bool IsDebuggingEnabled { get; set; }

        public IHttpRequest Request { get; set; }

        public IServer Server { get; set; }
    }
}
