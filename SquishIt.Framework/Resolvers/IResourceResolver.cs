namespace SquishIt.Framework.Resolvers
{
	/// <summary>
	/// Groups the various file/ folder resolvers.
	/// </summary>
	public interface IResourceResolver
	{
		IFolderResolver FileSystemResolver { get; }

		IFileResolver HttpResolver { get; }

		IFileResolver RootEmbeddedResourceResolver { get; }

		IFileResolver StandardEmbeddedResourceResolver { get; }
	}
}