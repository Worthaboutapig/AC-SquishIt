namespace SquishIt.Framework.Resolvers
{
    using System.Collections.Generic;

    /// <summary>
    /// Resolves filenames within a path, recursively if requested.
    /// </summary>
    public interface IFolderResolver : IFileResolver
    {
        /// <summary>
        /// Whether the provided path is conceptually a folder for this file resolver.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>True</c> if the path represents a folder in the abstracted storage mechanism, <c>false</c> otherwise.</returns>
        bool IsFolder(string path);

        /// <summary>
        /// Resolves all the files from the path in the resolver-held location to a physical filename.
        /// </summary>
        /// <param name="path">The path where the files are located.</param>
        /// <param name="recursive">Whether to recursively resolve files in child folders.</param>
        /// <param name="debugFileExtension">If a file ends with this extension, if will be excluded from the result set. A null value has no effect on the result set.</param>
        /// <param name="allowedFileExtensions">Files whose extension is in this list will be included in the returned enumerable. A null value has no effect on the result set.</param>
        /// <param name="disallowedFileExtensions">Files whose extension is in this list will be excluded from the returned enumerable. A null value has no effect on the result set.</param>
        /// <returns>The enumerable of all filenames in the path, per the restrictions in the parameters.</returns>
        IEnumerable<string> ResolveFilenames(string path, bool recursive, string debugFileExtension = null, IEnumerable<string> allowedFileExtensions = null, IEnumerable<string> disallowedFileExtensions = null);
    }
}