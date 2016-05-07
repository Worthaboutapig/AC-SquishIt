using System;
using System.Threading;

namespace SquishIt.Framework.Utilities
{
    public class CriticalRenderingSection : IDisposable
    {
        private readonly ITrustLevel _trustLevel;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CriticalRenderingSection(ITrustLevel trustLevel, IFilePathMutexProvider filePathMutexProvider, string path)
        {
            _trustLevel = trustLevel;

            if (_trustLevel.IsFullTrust)
            {
                _mutex = filePathMutexProvider.GetMutexForPath(path);
                _mutex.WaitOne();
            }
        }

        // Note: this feels a bit like IDisposable abuse but allows us to code BundleBase in a more mutex-agnostic fashion probably acceptable alternative for try .. finally though
        private readonly Mutex _mutex;

        public void Dispose()
        {
            if (_trustLevel.IsFullTrust)
            {
                _mutex.ReleaseMutex();
            }
        }
    }
}