using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using TypeSieve.Tests.ScannableAssembly.Compound.SubNamespace;

namespace TypeSieve.Tests
{
	public class NamespaceTests
	{
		[Fact]
		public void Static_Of_WhenCalledTwiceForSameType_ReturnsSameInstance()
		{
			Assert.Same(
				Namespace.Of<Object>(),
				Namespace.Of<Object>());
		}

		[Fact]
		public void GetNamespace_ReturnsNamespaceReferencingAssembly()
		{
			var subject = typeof(SubNamespaceModule);

			var ns = subject.GetNamespace();

			Assert.Same(
				subject.Assembly,
				ns.Assembly);
		}

		[Fact]
		public void GetNamespace_ReturnsNamespaceWithNamespaceString()
		{
			var subject = typeof(SubNamespaceModule);

			var ns = subject.GetNamespace();

			Assert.Equal(
				subject.Namespace,
				ns.NamespacePath);
		}

		[Fact]
		public void GetNamespace_ReturnsNamespaceReferencingSiblingTypes()
		{
			var subject = typeof(SubNamespaceModule);

			var ns = subject.GetNamespace();

			var siblingTypes =
				subject.Assembly.GetTypes()
					.Where(type => type.Namespace == subject.Namespace)
					.ToArray();

			foreach (var siblingType in siblingTypes)
			{
				Assert.Contains(
					siblingType,
					ns.Types);
			}
		}
	}
}
