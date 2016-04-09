//using System;
//using System.Collections.Generic;
//using SquishIt.Framework.Base;
//using SquishIt.Framework.Resolvers;

//namespace SquishIt.Framework.Files
//{


//    public class Input
//    {
//        public string Path { get; private set; }
//        public IFolderResolver Resolver { get; private set; }
//        public bool IsRecursive { get; private set; }

//        public Input(string filePath, bool recursive, IFolderResolver resolver)
//        {
//            Path = filePath;
//            IsRecursive = recursive;
//            Resolver = resolver;
//        }

//        public bool IsFolder { get { return Resolver.IsFolder(Path); } }

//        public IEnumerable<string> ResolveFiles(IEnumerable<string> allowedExtensions, IEnumerable<string> disallowedExtensions, string debugFileExtension)
//        {
//            if (IsFolder)
//            {
//                return Resolver.ResolveFilenames(Path, IsRecursive, debugFileExtension, allowedExtensions, disallowedExtensions);
//            }
//            else
//            {
//                return new[] {Resolver.ResolveFilename(Path)};
//            }
//        }

//        public static Input FromAsset(Asset asset, IPathTranslator pathTranslator, Func<bool> isDebuggingEnabled)
//        {
//            if (!asset.IsEmbeddedResource)
//            {
//                if (isDebuggingEnabled())
//                {
//                    return new Input(pathTranslator.ResolveAppRelativePathToFileSystem(asset.LocalPath), asset.IsRecursive, ResolverFactory.Get<FileSystemResolver>());
//                }
//                if (asset.IsRemoteDownload)
//                {
//                    return new Input(asset.RemotePath, false, ResolverFactory.Get<HttpResolver>());
//                }
//                //this is weird - do we absolutely need to treat as the remote downloads as local when debugging?
//                return new Input(pathTranslator.ResolveAppRelativePathToFileSystem(asset.LocalPath), asset.IsRecursive, ResolverFactory.Get<FileSystemResolver>());
//            }

//            return asset.IsEmbeddedInRootNamespace
//                ? new Input(asset.RemotePath, false, ResolverFactory.Get<RootEmbeddedResourceResolver>())
//                : new Input(asset.RemotePath, false, ResolverFactory.Get<StandardEmbeddedResourceResolver>());
//        }
//    }
//}