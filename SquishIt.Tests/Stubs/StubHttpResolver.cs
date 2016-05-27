using System;
using System.Collections.Generic;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Tests.Stubs
{
    public class StubHttpResolver : IFileResolver
    {
        private IDictionary<string, bool> _isDirectoryResolutions;
        private IDictionary<string, string> _fileResolutions;
        private IDictionary<string, string> _resolveFolderResolutions;

        public StubHttpResolver()
        {
            _isDirectoryResolutions = new Dictionary<string, bool>();
            _fileResolutions = new Dictionary<string, string>();
            _resolveFolderResolutions = new Dictionary<string, string>();
        }

        public void AddIsFolderResolution(string path, bool isDirectory = false)
        {
            _isDirectoryResolutions[path] = isDirectory;
        }

        public bool IsFolder(string path)
        {
            if (!_isDirectoryResolutions.ContainsKey(path))
            {
                throw new ArgumentOutOfRangeException("path", string.Format("No 'IsFolder' resolution has been added for path '{0}', call 'AddIsFolderResolution()' for this path.", path));
            }

            var isDirectory = _isDirectoryResolutions[path];

            return isDirectory;
        }

        public void AddResolveFileResolution(string file, string resolvesTo)
        {
            _fileResolutions[file] = resolvesTo;
        }

        public string ResolveFilename(string filePath)
        {
            if (!_fileResolutions.ContainsKey(filePath))
            {
                throw new ArgumentOutOfRangeException("filePath", string.Format("No 'Resolve' resolution has been added for path '{0}', call 'AddResolveFileResolution()' for this file.", filePath));
            }

            var resolvesTo = _fileResolutions[filePath];

            return resolvesTo;
        }

        public IEnumerable<string> ResolveFolder(string path, bool recursive, string debugFileExtension, IEnumerable<string> allowedExtensions, IEnumerable<string> disallowedExtensions)
        {
            throw new NotImplementedException();
        }
    }
}
