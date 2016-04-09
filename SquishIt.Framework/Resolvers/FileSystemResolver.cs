using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SquishIt.Framework.Resolvers
{
    /// <summary>
    /// Resolves files from a file system.
    /// </summary>
    public class FileSystemResolver : IFolderResolver
    {
        public string ResolveFilename(string filePath)
        {
            return Path.GetFullPath(filePath);
        }

        /// <summary>
        /// Whether the provided path is a file system folder.
        /// </summary>
        /// <param name="path">The path to check.</param>
        /// <returns><c>True</c> if the path is a folder in file system, <c>false</c> otherwise.</returns>
        public bool IsFolder(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// Resolves all the files from the path in the resolver-held location to a physical filename.
        /// </summary>
        /// <param name="path">The path where the files are located.</param>
        /// <param name="recursive">Whether to recursively resolve files in child folders.</param>
        /// <param name="debugFileExtension">If a file ends with this extension, if will be excluded from the result set. A null value has no effect on the result set.</param>
        /// <param name="allowedFileExtensions">Files whose extension is in this list will be included in the returned enumerable. A null value has no effect on the result set.</param>
        /// <param name="disallowedFileExtensions">Files whose extension is in this list will be excluded from the returned enumerable. A null value has no effect on the result set.</param>
        /// <returns>The enumerable of all filenames in the path, per the restrictions in the parameters.</returns>
        public IEnumerable<string> ResolveFilenames(string path, bool recursive, string debugFileExtension = null, IEnumerable<string> allowedFileExtensions = null, IEnumerable<string> disallowedFileExtensions = null)
        {
            if (IsFolder(path))
            {
                var files = Directory.GetFiles(path, "*.*", recursive
                    ? SearchOption.AllDirectories
                    : SearchOption.TopDirectoryOnly);
                var orderedFiles = files
                    .Where(file =>
                           {
                               var f = file.ToUpperInvariant();
                               // Files with a debug extension are never included.
                               if (debugFileExtension != null && f.EndsWith(debugFileExtension.ToUpperInvariant()))
                               {
                                   return false;
                               }

                               // Disallowed extensions override allowed extensions (principle of least-priviledge).
                               var extensionIsDisallowed = disallowedFileExtensions != null && disallowedFileExtensions.Select(s => s.ToUpper()).Any(f.EndsWith);
                               if (extensionIsDisallowed)
                               {
                                   return false;
                               }

                               var extensionIsAllowed = allowedFileExtensions == null || allowedFileExtensions.Select(s => s.ToUpper()).Any(f.EndsWith);

                               // The filename is included if it's not a debug filename
                               // AND (if there is a list of disallowed names, then the filename IS NOT in that list)
                               // AND (if there is a list of allowed names, then the filename IS in that list).
                               return extensionIsAllowed;
                           })
                    .OrderBy(k => k, Comparer<string>.Default);

                foreach (var file in orderedFiles)
                {
                    yield return file;
                }
            }
            else
            {
                yield return Path.GetFullPath(path);
            }
        }

        static IEnumerable<string> Extensions(string path)
        {
            return path
                .Split('.')
                .Skip(1)
                .Select(s => "." + s.ToUpperInvariant());
        }
    }
}