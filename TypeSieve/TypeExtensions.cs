using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve
{
	public static class TypeExtensions
	{
		public static Boolean HasInterface<TInterface>(this Type type)
		{
			return type.HasInterface(typeof(TInterface));
		}

		public static Boolean HasInterface(this Type type, Type interfaceType)
		{
			if (!interfaceType.IsInterface)
				throw new ArgumentException(
					String.Format(
						"Specified type {0} is not an interface.",
						interfaceType.FullName),
					"interfaceType");

			return
				type.GetAllInterfaces().Any(availableInterface => availableInterface.Equals(interfaceType));
		}

		public static Boolean HasOpenGenericInterface(this Type type, Type interfaceType)
		{
			if (!interfaceType.IsInterface)
				throw new ArgumentException(
					String.Format(
						"Specified type {0} is not an interface.",
						interfaceType.FullName),
					"interfaceType");

			if (!interfaceType.ContainsGenericParameters)
				throw new ArgumentException(
					String.Format(
						"Specified type {0} is not an open generic type.",
						interfaceType.FullName),
					"interfaceType");

			return
				type.GetAllInterfaces()
					.Where(availableInterface => availableInterface.IsGenericType)
					.Select(availableInterface => availableInterface.GetGenericTypeDefinition())
					.Any(openGenericInterface => openGenericInterface.Equals(interfaceType));
		}

		public static IEnumerable<Type> GetAllInterfaces(this Type type)
		{
			var interfaces = type.GetInterfaces();
			var inheritedInterfaces =
				from interfaceType in interfaces
				from inheritedInterface in interfaceType.GetAllInterfaces()
				select inheritedInterface;
			var baseTypeInterfaces =
				type.BaseType == null
					? Enumerable.Empty<Type>()
					: type.BaseType.GetAllInterfaces();

			return
				interfaces
					.Concat(inheritedInterfaces)
					.Concat(baseTypeInterfaces)
					.Distinct();
		}

		public static IEnumerable<Type> GetAllOpenGenericInterfaces(this Type type)
		{
			return
				type.GetAllInterfaces()
					.Where(interfaceType => interfaceType.IsGenericType)
					.Select(genericInterface => genericInterface.GetGenericTypeDefinition())
					.Distinct();
		}
	}
}
