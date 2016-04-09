using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.IO;
using System.Reflection;
using SquishIt.Framework.Utilities;

namespace SquishIt.Framework.Resolvers
{
    public abstract class EmbeddedResourceResolver : IFileResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected EmbeddedResourceResolver(ITempPathProvider tempPathProvider)
        {
            Contract.Requires(tempPathProvider != null);

            Contract.Ensures(_tempPathProvider != null);

            _tempPathProvider = tempPathProvider;
        }

        private readonly ITempPathProvider _tempPathProvider;

        protected abstract string CalculateResourceName(string assemblyName, string resourceName);

        public string ResolveFilename(string filePath)
        {
            var split = filePath.Split(new[] {"://"}, StringSplitOptions.None);
            var assemblyName = split.ElementAt(0);
            var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(x => x.GetName().Name == assemblyName);

            var filename = split.ElementAt(1);

            var resourceName = CalculateResourceName(assemblyName, filename);

            string resolved;
            if (TempFileResolutionCache.TryGetValue(resourceName, out resolved))
            {
                return resolved;
            }

            var fileName = ResolveFile(filePath, assembly, resourceName, filename);

            return fileName;
        }

        private string ResolveFile(string filePath, Assembly assembly, string resourceName, string filename)
        {
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException(string.Format("Embedded resource not found: {0}", filePath));
                }

                string contents;
                // Read the file contents from the embedded resource...
                using (var sr = new StreamReader(stream))
                {
                    contents = sr.ReadToEnd();
                }
                var fileName = _tempPathProvider.ForFile() + "-" + filename;

                // ...and write it to the temporary file.
                using (var sw = new StreamWriter(fileName))
                {
                    sw.Write(contents);
                }

                TempFileResolutionCache.Add(resourceName, fileName);

                return fileName;
            }
        }
    }
}