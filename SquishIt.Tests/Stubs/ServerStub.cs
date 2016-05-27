using System;
using System.IO;
using SquishIt.Framework.Web;

namespace SquishIt.Tests.Stubs
{
    public class ServerStub : IServer
    {
        private readonly string _applicationPhysicalPath;
        private readonly string _virtualSitePhysicalPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitePhysicalPath">The absolute path to the website.</param>
        /// <param name="virtualPath">This must not be a relative path.</param>
        /// <param name="applicationPhysicalPath">The absolute path to the application site, if there is one.  Set to the <paramref name="sitePhysicalPath"/> if null.</param>
        public ServerStub(string sitePhysicalPath, string virtualPath = null, string applicationPhysicalPath = null)
        {
            if (sitePhysicalPath == null) throw new ArgumentNullException("sitePhysicalPath");

            _applicationPhysicalPath = sitePhysicalPath;
            _virtualSitePhysicalPath = applicationPhysicalPath ?? Path.Combine(_applicationPhysicalPath, virtualPath ?? "");
        }

        public string MapPath(string path)
        {
            string adjustedPath;

            if (string.IsNullOrEmpty(path))
            {
                adjustedPath = ".";
            }
            else if (path.StartsWith("~"))
            {
                adjustedPath = path.StartsWith("~/") ? path.Substring(2) : path.Substring(1);
            }
            else
            {
                adjustedPath = path;
            }

            var rootPath = adjustedPath.StartsWith("/") ? _applicationPhysicalPath : _virtualSitePhysicalPath;
            adjustedPath = adjustedPath.TrimStart('/').TrimEnd('/');

            var mapPath = Path.Combine(rootPath, adjustedPath);

            return mapPath;
        }
    }
}