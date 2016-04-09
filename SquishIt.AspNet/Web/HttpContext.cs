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
		private IHttpRequest _httpRequest;
		private IServer _server;

		/// <summary>
		/// Abstracts a given <see cref="HttpContext"/>.
		/// </summary>
		/// <param name="httpContext"></param>
		public HttpContext(System.Web.HttpContext httpContext)
		{
			_httpContextWrapper = httpContext == null ? null : new HttpContextWrapper(httpContext);
		}

		/// <summary>
		/// The current HTTP request.
		/// </summary>
		public IHttpRequest Request { get { return  _httpRequest ?? (_httpRequest = _httpContextWrapper == null ? null : new HttpRequest(_httpContextWrapper.Request)); } }

		/// <summary>
		/// The server.
		/// </summary>
		public IServer Server { get { return _server ?? (_server = _httpContextWrapper == null ? null : new Server(_httpContextWrapper.Server)); } }

		/// <summary>
		/// Indicates whether the current HTTP request is in debug mode.
		/// </summary>
		/// <returns>
		/// <c>True</c> if the request is in debug mode, <c>false</c> otherwise.
		/// </returns>
		public bool IsDebuggingEnabled { get { return _httpContextWrapper == null ? false : _httpContextWrapper.IsDebuggingEnabled; } }
	}
}