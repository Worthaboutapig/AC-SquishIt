using Nancy;
using System;
using SquishIt.Framework;

namespace SquishIt.Nancy
{
	/// <summary>
	/// The default Nancy path translator.
	/// </summary>
	public class DefaultPathTranslator : IPathTranslator
	{
		private readonly IRootPathProvider _rootPathProvider;

		public DefaultPathTranslator(IRootPathProvider rootPathProvider)
		{
			_rootPathProvider = rootPathProvider;
		}

		public string ResolveAppRelativePathToFileSystem(string file)
		{
			// Remove query string
			if (file.IndexOf('?') != -1)
			{
				file = file.Substring(0, file.IndexOf('?'));
			}

			return _rootPathProvider.GetRootPath() + "/" + file.TrimStart('~').TrimStart('/');
		}

		public string ResolveFileSystemPathToAppRelative(string file)
		{
			var root = new Uri(_rootPathProvider.GetRootPath());
			return root.MakeRelativeUri(new Uri(file, UriKind.RelativeOrAbsolute)).ToString();
		}
		public string BuildAbsolutePath(string siteRelativePath)
		{
			throw new NotImplementedException();
		}
	}
}