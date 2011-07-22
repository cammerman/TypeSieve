using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;
using System.Collections;

namespace TypeSieve.Tests.TypeDetection
{
	public class TypeExtensionsTests
	{
		[Fact]
		public void HasInterface_T_GivenNonInterface_ThrowsArgumentException()
		{
			var ex =
				Assert.Throws<ArgumentException>(
					() => typeof(List<int>).HasInterface<Object>());

			Assert.Equal("interfaceType", ex.ParamName);
		}

		[Fact]
		public void HasInterface_T_GivenInterfaceNotImplemented_ReturnsFalse()
		{
			Assert.False(
				typeof(List<int>).HasInterface<IDictionary>());
		}

		[Fact]
		public void HasInterface_T_GivenInterfaceImplemented_ReturnsTrue()
		{
			Assert.True(
				typeof(List<int>).HasInterface<IList>());
		}

		[Fact]
		public void HasInterface_T_GivenGenericInterfaceImplemented_ReturnsTrue()
		{
			Assert.True(
				typeof(List<int>).HasInterface<IList<int>>());
		}

		[Fact]
		public void HasInterface_GivenNonInterface_ThrowsArgumentException()
		{
			var ex =
				Assert.Throws<ArgumentException>(
					() => typeof(List<int>).HasInterface(typeof(Object)));

			Assert.Equal("interfaceType", ex.ParamName);
		}

		[Fact]
		public void HasInterface_GivenInterfaceNotImplemented_ReturnsFalse()
		{
			Assert.False(
				typeof(List<int>).HasInterface(typeof(IDictionary)));
		}

		[Fact]
		public void HasInterface_GivenInterfaceImplemented_ReturnsTrue()
		{
			Assert.True(
				typeof(List<int>).HasInterface(typeof(IList)));
		}

		[Fact]
		public void HasInterface_GivenGenericInterfaceImplemented_ReturnsTrue()
		{
			Assert.True(
				typeof(List<int>).HasInterface(typeof(IList<int>)));
		}

		[Fact]
		public void HasOpenGenericInterface_GivenNonInterface_ThrowsArgumentException()
		{
			var ex =
				Assert.Throws<ArgumentException>(
					() => typeof(List<int>).HasOpenGenericInterface(typeof(Object)));

			Assert.Equal("interfaceType", ex.ParamName);
		}

		[Fact]
		public void HasOpenGenericInterface_GivenNonGenericInterface_ThrowsArgumentException()
		{
			var ex =
				Assert.Throws<ArgumentException>(
					() => typeof(List<int>).HasOpenGenericInterface(typeof(IEnumerable)));

			Assert.Equal("interfaceType", ex.ParamName);
		}

		[Fact]
		public void HasOpenGenericInterface_GivenOpenInterfaceNotImplemented_ReturnsFalse()
		{
			Assert.False(
				typeof(List<int>).HasOpenGenericInterface(typeof(IDictionary<,>)));
		}

		[Fact]
		public void HasOpenGenericInterface_GivenOpenInterfaceImplemented_ReturnsTrue()
		{
			Assert.True(
				typeof(List<int>).HasOpenGenericInterface(typeof(IList<>)));
		}

		[Fact]
		public void GetAllInterfaces_ReturnsAllInterfacesImplementedAcrossInheritanceHierarchy()
		{
			var interfacesOfGenericList =
				new Type[] {
					typeof(IList<int>),
					typeof(ICollection<int>),
					typeof(IEnumerable<int>),
					typeof(IList),
					typeof(ICollection),
					typeof(IEnumerable) };

			CollectionAssert.Equivalent(
				interfacesOfGenericList,
				typeof(List<int>).GetAllInterfaces().ToList());
		}

		[Fact]
		public void GetAllOpenGenericInterfaces_ReturnsOpenGenericTypesForAllGenericInterfacesImplementedAcrossInheritanceHierarchy()
		{
			var openGenericInterfacesOfGenericList =
				new Type[] {
					typeof(IList<>),
					typeof(ICollection<>),
					typeof(IEnumerable<>) };

			CollectionAssert.Equivalent(
				openGenericInterfacesOfGenericList,
				typeof(List<int>).GetAllOpenGenericInterfaces().ToList());
		}
	}
}
