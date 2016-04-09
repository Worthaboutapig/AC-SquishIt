using System;
using System.Collections.Specialized;
using SquishIt.Framework.Web;

namespace SquishIt.Tests.Web
{
    public class HttpRequestStub : IHttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HttpRequestStub(string applicationPath = "/", NameValueCollection queryString = null, Uri url = null)
        {
            ApplicationPath = applicationPath;
            QueryString = queryString ?? new NameValueCollection();
            Url = url ?? new Uri("http://localhost/");
        }

        public string ApplicationPath { get; set; }

        public NameValueCollection QueryString { get; set; }

        public Uri Url { get; set; }
    }
}