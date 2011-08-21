using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeSieve.AssemblyScan;

namespace TypeSieve.Tests
{
	public class TypeListOpinion : IInclusionOpinion
	{
		protected virtual HashSet<Type> Inclusions
		{
			get;
			private set;
		}

		protected virtual HashSet<Type> Exclusions
		{
			get;
			private set;
		}

		public TypeListOpinion(IEnumerable<Type> inclusions, IEnumerable<Type> exclusions)
		{
			var inclusionsSet = new HashSet<Type>(inclusions ?? new Type[] { });
			var exclusionsSet = new HashSet<Type>(exclusions ?? new Type[] { });

			var intersection = inclusionsSet.Join(exclusionsSet, outer => outer, inner => inner, (outer, inner) => true);

			if (intersection.Any())
				throw new Exception("A type can only be either included or excluded, not both.");

			Inclusions = inclusionsSet;
			Exclusions = exclusionsSet;
		}

		public EInclusionOpinion AdviseOn(Type type)
		{
			if (Inclusions.Contains(type))
				return EInclusionOpinion.Include;

			if (Exclusions.Contains(type))
				return EInclusionOpinion.Exclude;

			return EInclusionOpinion.Indifferent;
		}
	}
}
