using System;
using System.IO;
using SquishIt.Framework;

namespace SquishIt.AspNet
{
	using System.Diagnostics.Contracts;
	using Framework.Web;

	public class DefaultPathTranslator : IPathTranslator
	{
		private readonly IVirtualPathUtility _virtualPathUtility;
		private readonly IHttpContext _httpContext;
		private readonly string _virtualPathRoot;
		private readonly string _physicalPathRoot;

		public DefaultPathTranslator(string virtualVirtualPathRoot, string physicalPathRoot, IHttpContext httpContext = null, IVirtualPathUtility virtualPathUtility = null)
		{
			Contract.Requires(!string.IsNullOrWhiteSpace(virtualVirtualPathRoot));
			Contract.Requires(!string.IsNullOrWhiteSpace(virtualVirtualPathRoot));

			Contract.Ensures(!string.IsNullOrWhiteSpace(_virtualPathRoot));
			Contract.Ensures(!string.IsNullOrWhiteSpace(_physicalPathRoot));

			_virtualPathRoot = virtualVirtualPathRoot;
			_physicalPathRoot = physicalPathRoot;
			_httpContext = httpContext ?? Configuration.Instance.DefaultHttpContext();
			_virtualPathUtility = virtualPathUtility ?? Configuration.Instance.DefaultVirtualPathUtility();
		}

		public string ResolveAppRelativePathToFileSystem(string file)
		{
			// Remove query string
			if (file.IndexOf('?') != -1)
			{
				file = file.Substring(0, file.IndexOf('?'));
			}

			var path = _httpContext == null || _httpContext.Server == null
				? ProcessWithoutHttpContext(file)
				: _httpContext.Server.MapPath(Path.Combine(_virtualPathRoot, "/") + file.TrimStart("~/").TrimStart("~"));

			return path;
		}

		private static string ProcessWithoutHttpContext(string file)
		{
			file = Platform.Unix
				? file.TrimStart('~', '/') // does this need to stay this way to account for multiple leading slashes or can string trimstart be used?
				: file.Replace("/", "\\").TrimStart('~').TrimStart('\\');

			var path = Path.Combine(Environment.CurrentDirectory, file);

			return path;
		}

		public string ResolveFileSystemPathToAppRelative(string file)
		{
			if (_httpContext == null)
			{
				var root = new Uri(Environment.CurrentDirectory);
				var path = root.MakeRelativeUri(new Uri(file, UriKind.RelativeOrAbsolute)).ToString();
				var path2 = path.Substring(path.IndexOf("/", StringComparison.InvariantCulture) + 1);

				return path2;
			}
			else
			{
				var root = new Uri(_physicalPathRoot);
				var path = root.MakeRelativeUri(new Uri(file, UriKind.RelativeOrAbsolute)).ToString();

				return path;
			}
		}

		public string BuildAbsolutePath(string siteRelativePath)
		{
			if (_httpContext == null)
			{
				throw new InvalidOperationException("Absolute path can only be constructed in the presence of an HttpContext.");
			}

			if (!siteRelativePath.StartsWith("/"))
			{
				throw new InvalidOperationException("This helper method only works with site relative paths.");
			}

			var url = _httpContext.Request.Url;
			var port = url.Port == 80 ? string.Empty : ":" + url.Port;
			var path = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, _virtualPathUtility.ToAbsolute(siteRelativePath));

			return path;
		}
	}
}