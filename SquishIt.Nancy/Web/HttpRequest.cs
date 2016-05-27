using System;
using System.Collections.Specialized;
using Nancy;
using SquishIt.Framework.Web;

namespace SquishIt.Nancy.Web
{

	/// <summary>
	/// 
	/// </summary>
	public class HttpRequest : IHttpRequest
	{
		private readonly Request _request;
        private readonly IRootPathProvider _rootPathProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HttpRequest(Request request, IRootPathProvider rootPathProvider)
		{
			_request = request;
            _rootPathProvider = rootPathProvider;

		}

		public Uri Url { get { return _request.Url; } }
		public NameValueCollection QueryString { get { return _request.Query; } }
		public string ApplicationPath { get { return _rootPathProvider.GetRootPath(); } }
	}
}