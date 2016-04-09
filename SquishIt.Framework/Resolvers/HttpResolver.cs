using System.IO;
using System.Net;
using SquishIt.Framework.Utilities;

namespace SquishIt.Framework.Resolvers
{
    using System.Diagnostics.Contracts;

    public class HttpResolver : IFileResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public HttpResolver(ITempPathProvider tempPathProvider)
        {
            Contract.Requires(tempPathProvider != null);

            Contract.Ensures(_tempPathProvider != null);

            _tempPathProvider = tempPathProvider;
        }

        private readonly ITempPathProvider _tempPathProvider;

        public string ResolveFilename(string filePath)
        {
            string resolved;
            if (TempFileResolutionCache.TryGetValue(filePath, out resolved))
            {
                return resolved;
            }

            return ResolveWebResource(filePath);
        }

        private string ResolveWebResource(string path)
        {
            var webRequestObject = (HttpWebRequest) WebRequest.Create(path);
            var webResponse = webRequestObject.GetResponse();
            try
            {
                string contents;
                using (var sr = new StreamReader(webResponse.GetResponseStream()))
                {
                    contents = sr.ReadToEnd();
                }
                var fileName = _tempPathProvider.ForFile();

                using (var sw = new StreamWriter(fileName))
                {
                    sw.Write(contents);
                }

                TempFileResolutionCache.Add(path, fileName);

                return fileName;
            }
            finally
            {
                webResponse.Close();
            }
        }

        //public IEnumerable<string> ResolveFolder(string path, bool recursive, string debugFileExtension, IEnumerable<string> allowedExtensions, IEnumerable<string> disallowedExtensions)
        //{
        //    throw new NotImplementedException("Adding entire directories only supported by FileSystemResolver.");
        //}

        //public virtual bool IsFolder(string path)
        //{
        //    return false;
        //}
    }
}