using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public interface ISpecifyTypeSources
	{
		IFilterFromTypeSources FromTypes(IEnumerable<Type> types);

		IFilterFromTypeSources FromAssemblyContaining<TMarker>();

		IFilterFromTypeSources FromAssemblyContaining(params Type[] markers);

		IFilterFromTypeSources FromNamespaceContaining<TMarker>();

		IFilterFromTypeSources FromNamespaceContaining(params Type[] markers);
	}
}
