using System;
using SquishIt.Framework.Files;

namespace SquishIt.Framework.Renderers
{
    public class FileRenderer : IRenderer
    {
        protected IFileWriterFactory FileWriterFactory { get; }

        public FileRenderer(IFileWriterFactory fileWriterFactory)
        {
            if (fileWriterFactory == null) throw new ArgumentNullException("fileWriterFactory");

            FileWriterFactory = fileWriterFactory;
        }

        public void Render(string content, string outputFile)
        {
            WriteFiles(content, outputFile);
        }

        protected void WriteFiles(string output, string outputFile)
        {
            if (outputFile == null)
            {
                Console.WriteLine(output);
            }
            else
            {
                using (var fileWriter = FileWriterFactory.GetFileWriter(outputFile))
                {
                    fileWriter.Write(output);
                }
            }
        }
    }
}