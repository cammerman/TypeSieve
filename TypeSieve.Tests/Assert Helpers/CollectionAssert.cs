using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace TypeSieve.Tests
{
	public static class CollectionAssert
	{
		public static void Equivalent<T>(ICollection<T> expected, ICollection<T> actual)
		{
			Assert.Equal(expected.Count, actual.Count);

			foreach (var item in expected)
			{
				Assert.Contains(item, actual);
			}
		}
	}
}
