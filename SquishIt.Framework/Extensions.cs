using System;
using System.Collections.Generic;
using System.Linq;

namespace SquishIt.Framework
{
	/// <summary>
	/// Various extensions.
	/// </summary>
	public static class Extensions
	{
		public static string TrimStart(this string target, string toTrim)
		{
			var result = target;
			while (result.StartsWith(toTrim))
			{
				result = result.Substring(toTrim.Length);
			}

			return result;
		}

		public static string TrimEnd(this string target, string toTrim)
		{
			var result = target;
			while (result.EndsWith(toTrim))
			{
				result = result.Substring(0, result.Length - toTrim.Length);
			}

			return result;
		}

		/// <summary>
		/// Returns whether <paramref name="values"/> contains any element, returning <c>false</c> if <paramref name="values"/> is null.
		/// </summary>
		/// <typeparam name="T">The type.</typeparam>
		/// <param name="values">The enumerable to check.</param>
		/// <returns><c>False</c> if <paramref name="values"/> is null, otherwise it returns whether there are any items in the enumerable.</returns>
		public static bool NullSafeAny<T>(this IEnumerable<T> values)
		{
			var any = values != null && values.Any();

			return any;
		}

		/// <summary>
		/// Executes the given <paramref name="function"/> and returns the result unless <paramref name="function"/>, in which case returns <c>false</c>.
		/// </summary>
		/// <param name="function">The function to execute.</param>
		/// <returns><c>False</c> is <paramref name="function"/> is null, the return value of executing the function otherwise.</returns>
		public static bool SafeExecute(this Func<bool> function)
		{
			var succeeded = function != null && function();

			return succeeded;
		}
	}
}