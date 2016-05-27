using System;
using System.IO;

namespace SquishIt.Framework.Files
{
    public class FileWriter : IFileWriter
    {
        private readonly StreamWriter _streamWriter;

        public FileWriter(IRetryableFileOpener retryableFileOpener, string file, int numberOfRetries = 5)
        {
            if (retryableFileOpener == null) throw new ArgumentNullException("retryableFileOpener");
            if (string.IsNullOrWhiteSpace(file)) throw new ArgumentException("Invalid file name", "file");

            _streamWriter = retryableFileOpener.OpenTextStreamWriter(file, numberOfRetries, false);
        }

        public void Write(string value)
        {
            _streamWriter.Write(value);
        }

        public void WriteLine(string value)
        {
            _streamWriter.WriteLine(value);
        }

        public void Dispose()
        {
            if (_streamWriter != null)
            {
                _streamWriter.Dispose();
            }
        }
    }
}