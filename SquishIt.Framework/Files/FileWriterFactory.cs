using System;

namespace SquishIt.Framework.Files
{
    public class FileWriterFactory : IFileWriterFactory
    {
        protected IRetryableFileOpener RetryableFileOpener;
        protected int NumberOfRetries;

        public FileWriterFactory(IRetryableFileOpener retryableFileOpener, int numberOfRetries = 5)
        {
            if (retryableFileOpener == null) throw new ArgumentNullException("retryableFileOpener");

            RetryableFileOpener = retryableFileOpener;
            NumberOfRetries = numberOfRetries;
        }

        public IFileWriter GetFileWriter(string file)
        {
            var fileWriter = new FileWriter(RetryableFileOpener, file, NumberOfRetries);

            return fileWriter;
        }
    }
}