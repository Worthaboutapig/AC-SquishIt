using System;
using System.IO;

namespace SquishIt.Framework.Files
{
    public class FileReaderFactory : IFileReaderFactory
    {
        protected IRetryableFileOpener RetryableFileOpener;
        protected int NumberOfRetries;

        public FileReaderFactory(IRetryableFileOpener retryableFileOpener, int numberOfRetries = 5)
        {
            if (retryableFileOpener == null) throw new ArgumentNullException("retryableFileOpener");

            RetryableFileOpener = retryableFileOpener;
            NumberOfRetries = numberOfRetries;
        }

        public IFileReader GetFileReader(string file)
        {
            var fileReader = new FileReader(RetryableFileOpener, NumberOfRetries, file);

            return fileReader;
        }

        public bool FileExists(string file)
        {
            var fileExists = File.Exists(file);

            return fileExists;
        }
    }
}