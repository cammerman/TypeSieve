using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public interface ITypeSource
	{
		IEnumerable<Type> KnownTypes(IEnumerable<IInclusionOpinion> opinions);
	}
}
