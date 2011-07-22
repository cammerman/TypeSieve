using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public interface ISpecifyTypeSources
	{
		IFilterFromTypeSources FromAssemblyContaining<TMarker>();

		IFilterFromTypeSources FromNamespaceContaining<TMarker>();
	}
}
