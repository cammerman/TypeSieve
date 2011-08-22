using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public interface IFilterFromTypeSources : ISpecifyTypeSources
	{
		IFilterFromTypeSources ExcludeNamespaceContaining<TMarker>();
		IFilterFromTypeSources ExcludeNamespaceContaining(params Type[] markers);
		IFilterFromTypeSources IncludeNamespaceContaining<TMarker>();
		IFilterFromTypeSources IncludeNamespaceContaining(params Type[] markers);
		IFilterFromTypeSources IncludeTypesWhere(Func<Type, bool> select);
		IFilterFromTypeSources ExcludeTypesWhere(Func<Type, bool> filter);
		IFilterFromTypeSources TakeAdviceFrom(params IInclusionOpinion[] opinions);
	}
}
