using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	internal static class IEnumerableExtensions
	{
		public static IEnumerable<TItem> EmptyIfNull<TItem>(this IEnumerable<TItem> source)
		{
			if (source == null)
				return Enumerable.Empty<TItem>();

			return source;
		}

		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
		{
			var dictionary = new Dictionary<TKey, TValue>();

			foreach (var pair in source.EmptyIfNull())
			{
				dictionary[pair.Key] = pair.Value;
			}

			return dictionary;
		}
	}
}
