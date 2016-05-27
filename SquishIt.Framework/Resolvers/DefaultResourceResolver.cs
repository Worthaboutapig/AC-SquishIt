namespace SquishIt.Framework.Resolvers
{
    /// <summary>
    /// The default resource resolver.
    /// </summary>
    public class DefaultResourceResolver : IResourceResolver
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public DefaultResourceResolver(IFolderResolver fileSystemResolver, IFileResolver httpResolver, IFileResolver rootEmbeddedResourceResolver, IFileResolver standardEmbeddedResourceResolver)
        {
            FileSystemResolver = fileSystemResolver;
            HttpResolver = httpResolver;
            RootEmbeddedResourceResolver = rootEmbeddedResourceResolver;
            StandardEmbeddedResourceResolver = standardEmbeddedResourceResolver;
        }

        public IFolderResolver FileSystemResolver { get; }
        public IFileResolver HttpResolver { get; }
        public IFileResolver RootEmbeddedResourceResolver { get; }
        public IFileResolver StandardEmbeddedResourceResolver { get; }
    }
}
