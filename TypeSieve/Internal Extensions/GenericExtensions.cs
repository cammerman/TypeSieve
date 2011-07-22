using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	internal static class GenericExtensions
	{
		public static TSource ThrowIfNullArgument<TSource>(this TSource source, String argumentName)
		{
			if (source == null)
				throw new ArgumentNullException(argumentName);

			return source;
		}
	}
}
