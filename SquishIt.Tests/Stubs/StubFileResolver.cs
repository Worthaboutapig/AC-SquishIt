using System.Collections.Generic;
using SquishIt.Framework.Resolvers;
using System.Linq;

namespace SquishIt.Tests.Stubs
{
    public class StubFileResolver : IFolderResolver
    {
        string _pathToResolveTo;
        string[] _directoryContents;

        StubFileResolver(string pathToResolveTo, string[] directoryContents = null)
        {
            _pathToResolveTo = pathToResolveTo;
            _directoryContents = directoryContents;
        }

        public string ResolveFilename(string filePath)
        {
            return _pathToResolveTo;
        }

        public IEnumerable<string> ResolveFilenames(string path, bool recursive, string debugExtension, IEnumerable<string> allowedFileExtensions, IEnumerable<string> disallowedFileExtensions)
        {
            return _directoryContents
                .Where(
                        f => !f.ToUpperInvariant().EndsWith(debugExtension.ToUpperInvariant())
                            && (allowedFileExtensions == null
                            || allowedFileExtensions.Select(s => s.ToUpperInvariant()).Any(x => Extensions(f).Contains(x))
                            &&
                            (disallowedFileExtensions == null
                            || !disallowedFileExtensions.Select(s => s.ToUpperInvariant()).Any(x => Extensions(f).Contains(x)))
                            ))
                .ToArray();
        }

        public virtual bool IsFolder(string path)
        {
            return _directoryContents != null;
        }

        static IEnumerable<string> Extensions(string path)
        {
            return path.Split('.')
                .Skip(1)
                .Select(s => "." + s.ToUpperInvariant());
        }

        public static IFolderResolver ForDirectory(string[] files)
        {
            return new StubFileResolver(null, files);
        }

        public static IFolderResolver ForFile(string file)
        {
            return new StubFileResolver(file, null);
        }
    }
}