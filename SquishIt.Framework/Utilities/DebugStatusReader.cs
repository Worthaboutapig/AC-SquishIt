using System;
using SquishIt.Framework.Web;

namespace SquishIt.Framework.Utilities
{
    /// <summary>
    /// Sets and retrieves the current debug status.
    /// </summary>
    public class DebugStatusReader : IDebugStatusReader
    {
        private readonly IMachineConfigReader _machineConfigReader;
        private readonly IHttpContext _httpContext;
        private bool _forceDebug;
        private bool _forceRelease;
        private readonly ITrustLevel _trustLevel;

        /// <summary>
        /// Initialises the reader with the provided values.
        /// </summary>
        /// <param name="machineConfigReader">The machine configuration reader.</param>
        /// <param name="httpContext">The HTTP context.</param>
        public DebugStatusReader(IMachineConfigReader machineConfigReader, IHttpContext httpContext, ITrustLevel trustLevel)
        {
            _machineConfigReader = machineConfigReader;
            _httpContext = httpContext;
            _trustLevel = trustLevel;
        }

        /// <summary>
        /// Determines whether debugging is currently enabled for this reader.
        /// </summary>
        /// <param name="debugPredicate">An optional predicate to determine whether the reader is in debug mode. This overrides <see cref="ForceRelease"/>, but is overridden by <see cref="ForceDebug"/>, if it has been set to <c>true</c>.</param>
        /// <returns><c>True</c>, if the reader is in debug mode, <c>false</c> otherwise.</returns>
        public bool IsDebuggingEnabled(Func<bool> debugPredicate = null)
        {
            if (_forceDebug || debugPredicate.SafeExecute())
            {
                return true;
            }

            if (_forceRelease)
            {
                return false;
            }

            if (_httpContext != null && _httpContext.IsDebuggingEnabled)
            {
                var isDebuggingEnabled = !_trustLevel.IsHighOrUnrestrictedTrust || _machineConfigReader.IsNotRetailDeployment;
                return isDebuggingEnabled;
            }

            return false;
        }

        /// <summary>
        /// Forces the debug status reader into debug mode.
        /// </summary>
        public void ForceDebug()
        {
            _forceDebug = true;
        }

        /// <summary>
        /// Forces the debug status reader into release mode.
        /// </summary>
        public void ForceRelease()
        {
            _forceRelease = true;
        }
    }
}