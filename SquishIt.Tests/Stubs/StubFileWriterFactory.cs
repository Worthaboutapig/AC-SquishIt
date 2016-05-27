using System;
using System.Collections.Generic;
using SquishIt.Framework.Files;

namespace SquishIt.Tests.Stubs
{
    public class StubFileWriterFactory: IFileWriterFactory
    {
        readonly Dictionary<string, string> _files = new Dictionary<string, string>();

        public Dictionary<string, string> Files
        {
            get { return _files; }
        }

        public IFileWriter GetFileWriter(string file)
        {
            Action<string, string> writeDelegate = (f, contents) =>
                                   {
                                       if (_files.ContainsKey(f))
                                       {
                                           _files[f] = _files[f] + contents;
                                       }
                                       else
                                       {
                                           _files[f] = contents;
                                       }
                                   };

            var stubFileWriter = new StubFileWriter(file, writeDelegate);

            return stubFileWriter;
        }
    }
}