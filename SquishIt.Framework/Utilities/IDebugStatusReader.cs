using System;

namespace SquishIt.Framework.Utilities
{
    /// <summary>
    /// Sets and retrieves the current debug status.
    /// </summary>
    public interface IDebugStatusReader
    {
        /// <summary>
        /// Determines whether debugging is currently enabled for this reader.
        /// </summary>
        /// <param name="debugPredicate">An optional predicate to determine whether the reader is in debug mode.</param>
        /// <returns><c>True</c>, if the reader is in debug mode, <c>False</c> otherwise.</returns>
        bool IsDebuggingEnabled(Func<bool> debugPredicate = null);

        /// <summary>
        /// Forces the debug status reader into debug mode.
        /// </summary>
        void ForceDebug();

        /// <summary>
        /// Forces the debug status reader into release mode.
        /// </summary>
        void ForceRelease();
    }
}