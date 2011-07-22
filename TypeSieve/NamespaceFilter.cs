using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public class NamespaceFilter : FilterTypes
	{
		protected static bool FilterByNamespace(Type type, string ns)
		{
			return type.Namespace.StartsWith(ns);
		}

		public NamespaceFilter(EFilterMode mode, string ns)
			: base(mode, type => FilterByNamespace(type, ns))
		{
		}
	}
}
