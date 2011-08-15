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
		public void FromAssemblyContaining_T_StoresAssembly()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>();

			Assert.True(
				subject.KnownTypes()
					.Contains(typeof(IService)));
		}

		[Fact]
		public void FromNamespaceContaining_T_StoresAssembly()
		{
			var subject = new DetectTypes();

			subject.FromNamespaceContaining<IService>();

			Assert.True(
				subject.KnownTypes()
					.Contains(typeof(IService)));
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
