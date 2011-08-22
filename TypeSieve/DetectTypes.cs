using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TypeSieve.AssemblyScan
{
	public class DetectTypes : ISpecifyTypeSources, IFilterFromTypeSources, ITypeSource
	{
		private bool? _explicitlyInclusive = null;
		
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

		private readonly List<IInclusionOpinion> _filters;

		protected virtual IList<IInclusionOpinion> Filters
		{
			get { return _filters; }
		}

		private HashSet<Assembly> _assemblies;
		
		protected virtual ISet<Assembly> Assemblies
		{
			get { return _assemblies; }
		}

		private ISet<Namespace> _namespaces;

		protected ICollection<Namespace> Namespaces
		{
			get { return _namespaces; }
		}

		private ISet<Type> _types;

		protected ICollection<Type> Types
		{
			get { return _types; }
		}

		public DetectTypes()
		{
			_assemblies = new HashSet<Assembly>();
			_namespaces = new HashSet<Namespace>();
			_types = new HashSet<Type>();
			_filters = new List<IInclusionOpinion>();
		}

		public IFilterFromTypeSources FromTypes(IEnumerable<Type> types)
		{
			foreach (var type in types)
				Types.Add(type);

			return this;
		}

		public IFilterFromTypeSources FromAssemblyContaining<TMarker>()
		{
			return FromAssemblyContaining(typeof(TMarker));
		}

		public IFilterFromTypeSources FromAssemblyContaining(params Type[] markers)
		{
			foreach (var marker in markers)
				Assemblies.Add(marker.Assembly);

			return this;
		}

		public IFilterFromTypeSources FromNamespaceContaining<TMarker>()
		{
			return FromNamespaceContaining(typeof(TMarker));
		}

		public IFilterFromTypeSources FromNamespaceContaining(params Type[] markers)
		{
			foreach (var marker in markers)
				Namespaces.Add(Namespace.Of(marker));

			return this;
		}

		public IFilterFromTypeSources ExcludeNamespaceContaining<TMarker>()
		{
			return ExcludeNamespaceContaining(typeof(TMarker));
		}

		public IFilterFromTypeSources ExcludeNamespaceContaining(params Type[] markers)
		{
			Excluding();

			foreach (var marker in markers)
				_filters.Add(
					new NamespaceFilter(
						EFilterMode.Exclude,
						marker.Namespace));

			return this;
		}

		public IFilterFromTypeSources IncludeNamespaceContaining<TMarker>()
		{
			return IncludeNamespaceContaining(typeof(TMarker));
		}

		public IFilterFromTypeSources IncludeNamespaceContaining(params Type[] markers)
		{
			Including();

			foreach (var marker in markers)
				_filters.Add(
					new NamespaceFilter(
						EFilterMode.Include,
						marker.Namespace));

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

		protected virtual bool Included(Type type)
		{
			var included = !ExplicitlyInclusive;

			foreach (var filter in _filters)
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

		protected IEnumerable<Type> TypesInAssemblySources()
		{
			return
				from assembly in Assemblies
				from type in assembly.GetTypes()
				select type;
		}

		protected IEnumerable<Type> TypesInNamespaceSources()
		{
			return
				from ns in Namespaces
				from type in ns.Types
				select type;
		}

		protected IEnumerable<Type> TypesInAllSources()
		{
			return
				TypesInAssemblySources()
					.Concat(
						TypesInNamespaceSources())
					.Concat(Types);
		}

		public IFilterFromTypeSources TakeAdviceFrom(params IInclusionOpinion[] opinions)
		{
			_filters.AddRange(opinions);

			return this;
		}

		public IEnumerable<Type> KnownTypes()
		{
			return
				from type in TypesInAllSources().Distinct()
				where Included(type)
				select type;
		}
	}
}
