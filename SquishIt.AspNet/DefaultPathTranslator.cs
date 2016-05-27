using System;
using System.IO;
using SquishIt.Framework;
using SquishIt.Framework.Web;

namespace SquishIt.AspNet
{
    public class DefaultPathTranslator : IPathTranslator
    {
        private readonly IVirtualPathUtility _virtualPathUtility;
        private readonly IHttpContext _httpContext;
        private readonly string _applicationPhysicalPath;
        private readonly string _sitePhysicalPath;

        public DefaultPathTranslator(string sitePhysicalPath, IHttpContext httpContext, IVirtualPathUtility virtualPathUtility, string applicationPhysicalPath)
        {
            if (sitePhysicalPath == null) throw new ArgumentNullException("sitePhysicalPath");
            if (httpContext == null) throw new ArgumentNullException("httpContext");
            if (virtualPathUtility == null) throw new ArgumentNullException("virtualPathUtility");
            if (applicationPhysicalPath == null)
            {
                applicationPhysicalPath = sitePhysicalPath;
            }

            _sitePhysicalPath = sitePhysicalPath;
            _httpContext = httpContext;
            _virtualPathUtility = virtualPathUtility;
            _applicationPhysicalPath = applicationPhysicalPath;
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
                //Todo: Replace with TrimStart(new { '~/', '~' });?
                : _httpContext.Server.MapPath(Path.Combine(_applicationPhysicalPath, "/") + file.TrimStart("~/").TrimStart("~"));

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
                var root = new Uri(_sitePhysicalPath);
                var path = root.MakeRelativeUri(new Uri(file, UriKind.RelativeOrAbsolute)).ToString();

                return path;
            }
        }

        public string BuildAbsolutePath(string siteRelativePath)
        {
            if (!siteRelativePath.StartsWith("/"))
            {
                throw new InvalidOperationException("The path must be a relative path.");
            }

            var url = _httpContext.Request.Url;
            var port = url.Port == 80 ? string.Empty : ":" + url.Port;
            var path = string.Format("{0}://{1}{2}{3}", url.Scheme, url.Host, port, _virtualPathUtility.ToAbsolute(siteRelativePath));

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
    }
}