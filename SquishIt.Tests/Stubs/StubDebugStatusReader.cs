using System;
using SquishIt.Framework;
using SquishIt.Framework.Utilities;

namespace SquishIt.Tests.Stubs
{
    public class StubDebugStatusReader: IDebugStatusReader
    {
        private bool _isDebuggingEnabled;

        public StubDebugStatusReader()
        {
            _isDebuggingEnabled = true;
        }

        public StubDebugStatusReader(bool isDebuggingEnabled)
        {
            _isDebuggingEnabled = isDebuggingEnabled;
        }

        public bool IsDebuggingEnabled(Func<bool> debugPredicate = null)
        {
            return _isDebuggingEnabled || debugPredicate.SafeExecute();
        }

        public void ForceDebug()
        {
            _isDebuggingEnabled = true;
        }

        public void ForceRelease()
        {
            _isDebuggingEnabled = false;
        }
    }
}