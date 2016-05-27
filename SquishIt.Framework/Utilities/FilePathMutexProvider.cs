using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;

namespace SquishIt.Framework.Utilities
{
    public class FilePathMutexProvider : IFilePathMutexProvider
    {
        private const string NullPathSurrogate = "<NULL>";
        private static readonly object CreateMutexLock = new object();

        private readonly Dictionary<string, Mutex> _pathMutexes;
        private readonly IHasher _hasher;
        private readonly IPathTranslator _pathTranslator;

        public FilePathMutexProvider(IHasher hasher, IPathTranslator pathTranslator)
        {
            if (hasher == null) throw new ArgumentNullException();
            if (pathTranslator == null) throw new ArgumentNullException();

            _pathMutexes = new Dictionary<string, Mutex>(StringComparer.Ordinal);
            _hasher = hasher;
            _pathTranslator = pathTranslator;
        }

        [SecuritySafeCritical]
        public Mutex GetMutexForPath(string path)
        {
            Mutex result;

            var normalizedPath = GetNormalizedPath(path);
            if(_pathMutexes.TryGetValue(normalizedPath, out result))
            {
                return result;
            }

            lock(CreateMutexLock)
            {
                if(_pathMutexes.TryGetValue(normalizedPath, out result))
                {
                    return result;
                }

                result = CreateSharableMutexForPath(normalizedPath);
                _pathMutexes[normalizedPath] = result;
            }

            return result;
        }

        private string GetNormalizedPath(string path)
        {
            if(string.IsNullOrEmpty(path))
            {
                return NullPathSurrogate;
            }

            // Normalize the path
            var fileSystemPath = _pathTranslator.ResolveAppRelativePathToFileSystem(path);
            // The path is lower cased to avoid different hashes. Even on a case sensitive
            // file system this probably is okay, since it's a web application
            return Path.GetFullPath(fileSystemPath)
                .ToLowerInvariant();
        }

        [SecuritySafeCritical]
        private Mutex CreateSharableMutexForPath(string normalizedPath)
        {
            // The path is transformed to a hash value to avoid getting an invalid Mutex name.
            var mutexName = @"Global\SquishitPath" + _hasher.GetHash(normalizedPath);
            return CreateSharableMutex(mutexName);
        }

        [SecurityCritical]
        private static Mutex CreateSharableMutex(string name)
        {
            // Creates a mutex sharable by more than one process

            // The constructor will either create new mutex or open
            // an existing one, in a thread-safe manner
            bool createdNew;

            var everyoneSid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            if(Platform.Mono)
            {
                //MutexAccessRules don't seem to work on mono yet: http://lists.ximian.com/pipermail/mono-list/2008-August/039294.html
                return new Mutex(false, name, out createdNew);
            }
            var mutexSecurity = new MutexSecurity();
            mutexSecurity.AddAccessRule(new MutexAccessRule(everyoneSid, MutexRights.FullControl, AccessControlType.Allow));
            return new Mutex(false, name, out createdNew, mutexSecurity);
        }
    }
}
