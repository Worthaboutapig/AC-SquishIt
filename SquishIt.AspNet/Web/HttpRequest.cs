using System;
using System.Collections.Specialized;
using SquishIt.Framework.Web;

namespace SquishIt.AspNet.Web
{
	using System.Web;

	/// <summary>
	/// 
	/// </summary>
	public class HttpRequest : IHttpRequest
	{
		private readonly HttpRequestBase _httpRequestBase;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object"/> class.
		/// </summary>
		public HttpRequest(HttpRequestBase httpRequestBase)
		{
			_httpRequestBase = httpRequestBase;
		}

		public Uri Url { get { return _httpRequestBase.Url; } }
		public NameValueCollection QueryString { get { return _httpRequestBase.QueryString; } }
		public string ApplicationPath { get { return _httpRequestBase.ApplicationPath; } }
	}
}