using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using TypeSieve.AssemblyScan;
using TypeSieve.Tests.ScannableAssembly;

namespace TypeSieve.Tests
{
	public class DetectTypesTests
	{
		[Fact]
		public void KnownTypes_GivenFromAssemblyContainingHasBeenCalled_ReturnsTypesInAssemlby()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>();

			var expectedTypes = typeof(IService).Assembly.GetTypes();
			var knownTypes = subject.KnownTypes();

			foreach (var type in expectedTypes)
				Assert.Contains(
					type,
					knownTypes);
		}

		[Fact]
		public void KnownTypes_GivenFromNamespaceContainingHasBeenCalled_ReturnsTypesInAssembly()
		{
			var subject = new DetectTypes();

			subject.FromNamespaceContaining<IService>();
			
			var ns = typeof(IService).Namespace;
			var expectedTypes = typeof(IService).Assembly.GetTypes().Where(type => type.Namespace == ns);
			var knownTypes = subject.KnownTypes();

			foreach (var type in expectedTypes)
				Assert.Contains(
					type,
					knownTypes);
		}

		[Fact]
		public void FromAssemblyContaining_T_ReturnsSelfAsFilter()
		{
			var subject = new DetectTypes();

			Assert.Same(
				subject,
				subject.FromAssemblyContaining<IService>());
		}
	}
}
