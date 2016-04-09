//namespace SquishIt.Framework.Files
//{
//    using System;
//    using Base;
//    using Resolvers;

//    public class CompositeInputResolver
//    {
//        private FileSystemResolver _fileSystemResolver;
//        private HttpResolver _httpResolver;
//        private RootEmbeddedResourceResolver _rootEmbeddedResourceResolver;
//        private readonly StandardEmbeddedResourceResolver _standardEmbeddedResourceResolver;

//        private IPathTranslator _pathTranslator;

//        public InputResolver FromAsset(Asset asset, Func<bool> isDebuggingEnabled)
//        {
//            InputResolver inputResolver;
//            if (!asset.IsEmbeddedResource)
//            {
//                inputResolver = new InputResolver(_pathTranslator.ResolveAppRelativePathToFileSystem(asset.LocalPath), _fileSystemResolver);

//                if (isDebuggingEnabled())
//                {
//                    return inputResolver;
//                }
//                if (asset.IsRemoteDownload)
//                {
//                    inputResolver = new InputResolver(asset.RemotePath, _httpResolver);
//                    return inputResolver;
//                }
//                //this is weird - do we absolutely need to treat as the remote downloads as local when debugging?
//                return inputResolver;
//            }

//            if (asset.IsEmbeddedInRootNamespace)
//            {
//                inputResolver = new InputResolver(asset.RemotePath, _rootEmbeddedResourceResolver);
//            }
//            else
//            {
//                inputResolver = new InputResolver(asset.RemotePath, _standardEmbeddedResourceResolver);
//            }

//            return inputResolver;
//        }
//    }
//}