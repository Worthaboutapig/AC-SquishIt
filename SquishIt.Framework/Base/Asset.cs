using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SquishIt.Framework.Resolvers;

namespace SquishIt.Framework.Base
{
    public class Asset
    {
        internal string LocalPath { get; set; }
        internal string RemotePath { get; set; }
        internal int Order { get; set; }
        internal bool IsEmbeddedResource { get; set; }
        internal bool IsEmbeddedInRootNamespace { get; set; }
        internal bool DownloadRemote { get; set; }
        internal bool IsRecursive { get; set; }
        internal string Content { get; set; }
        internal string Extension { get; set; }
        internal bool Minify { get; set; }
        internal string ArbitraryWorkingDirectory { get; set; }

        internal bool IsArbitrary
        {
            get { return !string.IsNullOrEmpty(Content); }
        }

        internal bool IsLocal
        {
            get { return string.IsNullOrEmpty(RemotePath) && !IsArbitrary; }
        }

        internal bool IsRemote
        {
            get { return !string.IsNullOrEmpty(RemotePath) && !IsEmbeddedResource && !DownloadRemote; }
        }

        internal bool IsRemoteDownload
        {
            get { return !string.IsNullOrEmpty(RemotePath) && DownloadRemote; }
        }

        internal Asset()
        {
            Minify = true;
        }

        internal class FilenamesResolver
        {
            private readonly IPathTranslator _pathTranslator;
            private readonly IFolderResolver _fileSystemResolver;
            private readonly IFileResolver _httpResolver;
            private readonly IFileResolver _rootEmbeddedResourceResolver;
            private readonly IFileResolver _standardEmbeddedResourceResolver;

            public FilenamesResolver(IPathTranslator pathTranslator, IResourceResolver resourceResolver)
            {
                Contract.Requires(pathTranslator != null);
                Contract.Requires(resourceResolver != null);

                Contract.Ensures(_pathTranslator != null);
                Contract.Ensures(_fileSystemResolver != null);
                Contract.Ensures(_httpResolver != null);
                Contract.Ensures(_rootEmbeddedResourceResolver != null);
                Contract.Ensures(_standardEmbeddedResourceResolver != null);

                _pathTranslator = pathTranslator;
                _fileSystemResolver = resourceResolver.FileSystemResolver;
                _httpResolver = resourceResolver.HttpResolver;
                _rootEmbeddedResourceResolver = resourceResolver.RootEmbeddedResourceResolver;
                _standardEmbeddedResourceResolver = resourceResolver.StandardEmbeddedResourceResolver;
            }

            internal IEnumerable<string> ResolveFilenames(Asset asset, string debugFileExtension, IEnumerable<string> allowedExtensions, IEnumerable<string> disallowedExtensions, Func<bool> isDebuggingEnabled)
            {
                var files = new List<string>();
                if (asset.IsEmbeddedResource)
                {
                    var fileResolver = asset.IsEmbeddedInRootNamespace
                        ? _rootEmbeddedResourceResolver
                        : _standardEmbeddedResourceResolver;
                    files.Add(fileResolver.ResolveFilename(asset.RemotePath));
                }
                else
                {
                    if (isDebuggingEnabled() || !asset.IsRemoteDownload)
                    {
                        //this is weird - do we absolutely need to treat as the remote downloads as local when debugging?
                        files.AddRange(_fileSystemResolver.ResolveFilenames(_pathTranslator.ResolveAppRelativePathToFileSystem(asset.LocalPath), asset.IsRecursive, debugFileExtension, allowedExtensions, disallowedExtensions));
                    }
                    else
                    {
                        files.Add(_httpResolver.ResolveFilename(asset.RemotePath));
                    }
                }

                return files;
            }
        }

        private class AssetFileResolver : IFileResolver
        {
            private readonly string _filePath;
            private readonly IFileResolver _fileResolver;

            public AssetFileResolver(string filePath, IFileResolver fileResolver)
            {
                _fileResolver = fileResolver;
                _filePath = filePath;
            }

            public string ResolveFilename(string filePath)
            {
                return _fileResolver.ResolveFilename(filePath);
            }
        }

        private class AssetFolderResolver : IFolderResolver
        {
            private readonly IPathTranslator _pathTranslator;
            private readonly IFolderResolver _folderResolver;

            public AssetFolderResolver(IPathTranslator pathTranslator, IFolderResolver folderResolver)
            {
                _folderResolver = folderResolver;
                _pathTranslator = pathTranslator;
            }

            public bool IsFolder(string path)
            {
                return _folderResolver.IsFolder(path);
            }

            public string ResolveFilename(string filePath)
            {
                var file = _folderResolver.ResolveFilename(filePath);

                return file;
            }

            public IEnumerable<string> ResolveFilenames(string path, bool recursive, string debugFileExtension = null, IEnumerable<string> allowedFileExtensions = null, IEnumerable<string> disallowedFileExtensions = null)
            {
                var files = new List<string>();
                if (IsFolder(path))
                {
                    files.AddRange(_folderResolver.ResolveFilenames(path, recursive, debugFileExtension, allowedFileExtensions, disallowedFileExtensions));
                }
                else
                {
                    files.Add(_folderResolver.ResolveFilename(path));
                }

                return files;
            }
        }
    }
}