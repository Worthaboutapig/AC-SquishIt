using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using SquishIt.Framework;
using SquishIt.Framework.Utilities;

namespace SquishIt.Tests
{
    public abstract class MultipleWorkerProcessesTests
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        protected MultipleWorkerProcessesTests(IDebugStatusReader debugStatusReader)
        {
            Contract.Requires(debugStatusReader != null);

            Contract.Ensures(_debugStatusReader != null);

            _debugStatusReader = debugStatusReader;
        }

        private readonly IDebugStatusReader _debugStatusReader;

        [Test,Explicit]
        public void ExceptionIsThrowWhenMultipleWorkerThreadsAreWorking()
        {
            int writingThreads = 4;
            int readingThreads = 20;
            int numberOfExecutions = 500;

            for (int i = 0; i < numberOfExecutions; i++)
            {
                for (int y = 0; y < writingThreads; y++)
                {
                    Thread writingThread = new Thread(new ThreadStart(() =>
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine("Writing CSS...");

                            Bundle.Css(_debugStatusReader)
                                .Add("1.css")
                                .Add("2.css")
                                .Add("3.css")
                                .Add("4.css")
                                .Add("5.css")
                                .Add("6.css")
                                .ForceRelease()
                                .Render("combined_#.css");
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }
                    }));

                    writingThread.Start();
                }

                for (int y = 0; y < readingThreads; y++)
                {
                    Thread readingThread = new Thread(new ThreadStart(() =>
                    {
                        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "combined*.css");

                        var ii = 0;

                        while (ii < 1000)
                        {
                            System.Diagnostics.Debug.WriteLine("Reading CSS...");

                            files.ToList().ForEach(x => File.ReadAllText(x));

                            Thread.Sleep(50);
                            ii++;
                        }
                    }));

                    readingThread.Start();
                }
            }

            Thread.Sleep(20000);
        }

        /*public void ReadAllCssFiles(object state)
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "combined*.css");

            var i = 0;

            while (i < 2000)
            {
                System.Diagnostics.Debug.WriteLine("Reading CSS...");

                files.ToList().ForEach(x => File.ReadAllText(x));

                Thread.Sleep(50);
                i++;
            }
        }*/
    }
}
