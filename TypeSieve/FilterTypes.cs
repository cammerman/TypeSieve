using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.AssemblyScan
{
	public class FilterTypes : IInclusionOpinion
	{
		private readonly Func<Type, bool> _filter;
		
		private readonly EFilterMode _mode;
		public virtual EFilterMode Mode
		{
			get { return _mode; }
		}

		public FilterTypes(EFilterMode mode, Func<Type, bool> filter)
		{
			_mode = mode;
			_filter = filter.ThrowIfNullArgument("filter");
		}

		public EInclusionOpinion AdviseOn(Type type)
		{
			var matches = _filter(type);

			if (matches)
			{
				if (Mode == EFilterMode.Include)
					return EInclusionOpinion.Include;
				else if (Mode == EFilterMode.Exclude)
					return EInclusionOpinion.Exclude;

				return EInclusionOpinion.Indifferent;
			}
			else
			{
				return EInclusionOpinion.Indifferent;
			}
		}
	}
}
