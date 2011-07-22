using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TypeSieve.AssemblyScan
{
	public class DetectTypes : ISpecifyTypeSources, IFilterFromTypeSources, ITypeSource
	{
		private HashSet<Assembly> _assemblies;
		private bool? _explicitlyInclusive = null;
		private readonly List<FilterTypes> _filters;

		protected virtual bool ExplicitlyInclusive
		{
			get { return _explicitlyInclusive ?? false; }
		}

		protected virtual void Excluding()
		{
			if (_explicitlyInclusive == null)
				_explicitlyInclusive = false;
		}

		protected virtual void Including()
		{
			if (_explicitlyInclusive == null)
				_explicitlyInclusive = true;
		}

		protected virtual ISet<Assembly> Assemblies
		{
			get { return _assemblies; }
		}

		public virtual IEnumerable<Assembly> SourceAssemblies
		{
			get { return _assemblies; }
		}

		public DetectTypes()
		{
			_assemblies = new HashSet<Assembly>();
			_filters = new List<FilterTypes>();
		}

		public IFilterFromTypeSources FromAssemblyContaining<TMarker>()
		{
			Assemblies.Add(typeof(TMarker).Assembly);
			return this;
		}

		public IFilterFromTypeSources FromNamespaceContaining<TMarker>()
		{
			throw new NotImplementedException();
		}

		public IFilterFromTypeSources ExcludeNamespaceContaining<TMarker>()
		{
			Excluding();

			_filters.Add(
				new NamespaceFilter(
					EFilterMode.Exclude,
					typeof(TMarker).Namespace));

			return this;
		}

		public IFilterFromTypeSources IncludeNamespaceContaining<TMarker>()
		{
			Including();

			_filters.Add(
				new NamespaceFilter(
					EFilterMode.Include,
					typeof(TMarker).Namespace));

			return this;
		}

		public IFilterFromTypeSources IncludeTypesWhere(Func<Type, bool> select)
		{
			Including();

			_filters.Add(
				new FilterTypes(
					EFilterMode.Include,
					select));

			return this;
		}

		public IFilterFromTypeSources ExcludeTypesWhere(Func<Type, bool> filter)
		{
			Excluding();

			_filters.Add(
				new FilterTypes(
					EFilterMode.Exclude,
					filter));

			return this;
		}

		protected virtual bool Included(Type type, IEnumerable<IInclusionOpinion> additionalOpinions)
		{
			var included = !ExplicitlyInclusive;

			var allOpinions =
				additionalOpinions
					.EmptyIfNull()
					.Concat(
						_filters.Cast<IInclusionOpinion>());

			foreach (var filter in allOpinions)
			{
				var advice = filter.AdviseOn(type);

				if (!included && advice == EInclusionOpinion.Include)
				{
					included = true;
				}
				else if (included && advice == EInclusionOpinion.Exclude)
				{
					included = false;
				}
				else if (advice == EInclusionOpinion.Indifferent)
				{
					// Do nothing.
				}
			}

			return included;
		}

		public IEnumerable<Type> KnownTypes(IEnumerable<IInclusionOpinion> opinions)
		{
			return
				from assembly in SourceAssemblies
				from type in assembly.GetTypes()
				where Included(type, opinions)
				select type;
		}
	}
}
