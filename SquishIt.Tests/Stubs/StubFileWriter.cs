using System;
using SquishIt.Framework.Files;

namespace SquishIt.Tests.Stubs
{
    public class StubFileWriter: IFileWriter
    {
        private readonly string _file;
        private readonly Action<string,string> _writeDelegate;

        public StubFileWriter(string file, Action<string,string> writeDelegate)
        {
            if (file == null) throw new ArgumentNullException("file");

            _file = file;
            _writeDelegate = writeDelegate;
        }

        public void Dispose()
        {
        }

        public void Write(string value)
        {
            _writeDelegate(_file, value);
        }

        public void WriteLine(string value)
        {
            _writeDelegate(_file, value + Environment.NewLine);
        }
    }
}