using System.IO;
using Nancy;
using SquishIt.Framework.Web;

namespace SquishIt.Nancy.Web
{
    /// <summary>
	/// 
	/// </summary>
	public class Server : IServer
	{
		private readonly IRootPathProvider _rootPathProvider;

		public Server(IRootPathProvider rootPathProvider)
		{
			_rootPathProvider = rootPathProvider;
		}

		public string MapPath(string path)
		{
		    var rootPath = _rootPathProvider.GetRootPath();
            var mappedPath = Path.Combine(rootPath, path);

		    return mappedPath;
		}
	}
}