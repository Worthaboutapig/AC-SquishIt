namespace SquishIt.Framework.Resolvers
{
    /// <summary>
    /// Resolves a file from an abstracted storage mechanism, e.g. file-system, http, embedded resource.
    /// </summary>
    public interface IFileResolver
    {
        /// <summary>
        /// Resolves the given file from it's resolver-held location to a physical filename.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string ResolveFilename(string filePath);
   }
}