using System;
using System.Collections.Generic;
using System.Linq;
using SquishIt.Framework.Base;
using SquishIt.Framework.CSS;
using SquishIt.Framework.JavaScript;

namespace SquishIt.Framework
{
    using System.Diagnostics.Contracts;
    using Utilities;

    /// <summary>
    /// This is the entry point for the majority of interaction with the SquishIt API.
    /// </summary>
    public class Bundle
    {
        internal static readonly List<IPreprocessor> Preprocessors = new List<IPreprocessor>();

        internal static readonly HashSet<String> AllowedGlobalExtensions = new HashSet<string>();
        internal static readonly HashSet<String> AllowedScriptExtensions = new HashSet<string>
                                                                           {
                                                                               ".JS"
                                                                           };
        internal static readonly HashSet<String> AllowedStyleExtensions = new HashSet<string>
                                                                          {
                                                                              ".CSS"
                                                                          };

        /// <summary>
        /// Register a preprocessor instance to be used for all bundle types.
        /// </summary>
        /// <typeparam name="T"><see cref="IPreprocessor">IPreprocessor</see> implementation type.</typeparam>
        /// <param name="instance"><see cref="IPreprocessor">IPreprocessor</see> instance.</param>
        public static void RegisterGlobalPreprocessor<T>(T instance) where T : IPreprocessor
        {
            ValidatePreprocessor<T>(instance);
            foreach (var ext in instance.Extensions)
            {
                AllowedGlobalExtensions.Add(ext.ToUpper());
            }
            Preprocessors.Add(instance);
            if (instance.IgnoreExtensions.NullSafeAny())
            {
                foreach (var ext in instance.IgnoreExtensions)
                {
                    AllowedGlobalExtensions.Add(ext.ToUpper());
                }
                Preprocessors.Add(new NullPreprocessor(instance.IgnoreExtensions));
            }
        }

        /// <summary>
        /// Register a preprocessor instance to be used for script bundles.
        /// </summary>
        /// <typeparam name="T"><see cref="IPreprocessor">IPreprocessor</see> implementation type.</typeparam>
        /// <param name="instance"><see cref="IPreprocessor">IPreprocessor</see> instance.</param>
        public static void RegisterScriptPreprocessor<T>(T instance) where T : IPreprocessor
        {
            ValidatePreprocessor<T>(instance);
            foreach (var ext in instance.Extensions)
            {
                AllowedScriptExtensions.Add(ext.ToUpper());
            }
            Preprocessors.Add(instance);
            if (instance.IgnoreExtensions.NullSafeAny())
            {
                foreach (var ext in instance.IgnoreExtensions)
                {
                    AllowedScriptExtensions.Add(ext.ToUpper());
                }
                Preprocessors.Add(new NullPreprocessor(instance.IgnoreExtensions));
            }
        }

        /// <summary>
        /// Register a preprocessor instance to be used for all style bundles.
        /// </summary>
        /// <typeparam name="T"><see cref="IPreprocessor">IPreprocessor</see> implementation type.</typeparam>
        /// <param name="instance"><see cref="IPreprocessor">IPreprocessor</see> instance.</param>
        public static void RegisterStylePreprocessor<T>(T instance) where T : IPreprocessor
        {
            ValidatePreprocessor<T>(instance);
            foreach (var ext in instance.Extensions)
            {
                AllowedStyleExtensions.Add(ext.ToUpper());
            }
            Preprocessors.Add(instance);
            if (instance.IgnoreExtensions.NullSafeAny())
            {
                foreach (var ext in instance.IgnoreExtensions)
                {
                    AllowedStyleExtensions.Add(ext.ToUpper());
                }
                Preprocessors.Add(new NullPreprocessor(instance.IgnoreExtensions));
            }
        }

        static void ValidatePreprocessor<T>(IPreprocessor instance)
        {
            if (Preprocessors.Any(p => p.GetType() == typeof (T)))
            {
                throw new InvalidOperationException(string.Format("Can't add multiple preprocessors of type: {0}", typeof (T).FullName));
            }

            foreach (var extension in instance.Extensions)
            {
                if (AllExtensions.Contains(extension))
                {
                    throw new InvalidOperationException(string.Format("Can't add multiple preprocessors for extension: {0}", extension));
                }
            }
        }

        static IEnumerable<string> AllExtensions { get { return AllowedGlobalExtensions.Union(AllowedScriptExtensions).Union(AllowedStyleExtensions).Select(x => x.ToUpper()); } }

        public static void ClearPreprocessors()
        {
            Preprocessors.Clear();

            AllowedGlobalExtensions.Clear();
            AllowedScriptExtensions.Clear();
            AllowedStyleExtensions.Clear();

            AllowedScriptExtensions.Add(".JS");
            AllowedStyleExtensions.Add(".CSS");
        }
    }
}