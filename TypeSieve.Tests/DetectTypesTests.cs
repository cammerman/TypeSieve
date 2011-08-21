using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using TypeSieve.AssemblyScan;
using TypeSieve.Tests.ScannableAssembly;
using TypeSieve.Tests.ScannableAssembly.MultipleImplementors;
using TypeSieve.Tests.ScannableAssembly.Compound;
using TypeSieve.Tests.ScannableAssembly.ThreeDeep;
using TypeSieve.Tests.ScannableAssembly.MultipleImplementors.SubImplementor;

namespace TypeSieve.Tests
{
	public class DetectTypesTests
	{
		[Fact]
		public void KnownTypes_GivenFromAssemblyContainingHasBeenCalled_ReturnsTypesInAssembly()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>();

			var expectedTypes = typeof(IService).Assembly.GetTypes();
			var knownTypes = subject.KnownTypes();

			foreach (var type in expectedTypes)
				Assert.Contains(
					type,
					knownTypes);
		}

		[Fact]
		public void KnownTypes_GivenFromNamespaceContainingHasBeenCalled_ReturnsTypesInAssembly()
		{
			var subject = new DetectTypes();

			subject.FromNamespaceContaining<IService>();
			
			var ns = typeof(IService).Namespace;
			var expectedTypes = typeof(IService).Assembly.GetTypes().Where(type => type.Namespace == ns);
			var knownTypes = subject.KnownTypes();

			foreach (var type in expectedTypes)
				Assert.Contains(
					type,
					knownTypes);
		}

		[Fact]
		public void FromAssemblyContaining_T_ReturnsSelfAsFilter()
		{
			var subject = new DetectTypes();

			Assert.Same(
				subject,
				subject.FromAssemblyContaining<IService>());
		}
		
		[Fact]
		public void FromNamespaceContaining_T_ReturnsSelfAsFilter()
		{
			var subject = new DetectTypes();

			Assert.Same(
				subject,
				subject.FromNamespaceContaining<IService>());
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamespace_ReturnsNoTypesInExcludedNamespace()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();
			
			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(t => t.Namespace == markerType.Namespace)
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamespace_ReturnsAllTypesNotInExcludedNamespace()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(t => !t.Namespace.StartsWith(markerType.Namespace))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamespace_ReturnsNoTypesInExcludedSubNamespace()
		{
			var markerType = typeof(ICompoundNeed);

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<ICompoundNeed>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(t => t.Namespace.StartsWith(markerType.Namespace))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTypeCondition_ReturnsNoTypesMatchingCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(matchCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(matchCondition)
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTypeCondition_ReturnsAllTypesNotMatchingCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(matchCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !matchCondition(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingNamespace_ReturnsNoTypesOutsideIncludedNamespace()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(t => !t.Namespace.StartsWith(markerType.Namespace))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingNamespace_ReturnsAllTypesIncludedNamespace()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(t => t.Namespace.StartsWith(markerType.Namespace))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTypeCondition_ReturnsAllTypesMatchingCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(matchCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(matchCondition)
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTypeCondition_ReturnsNoTypesNotMatchingCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(matchCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !matchCondition(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		// Two of same filters
		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTwoNamespaces_ReturnsNoTypesInEitherExcludedNamespace()
		{
			var markerTypes = new[] { typeof(IGeneralInterface), typeof(IThreeDeepNeed) };
			var markerTypeNamespaces = markerTypes.Select(type => type.Namespace).ToList();

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>()
				.ExcludeNamespaceContaining<IThreeDeepNeed>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => markerTypeNamespaces.Contains(t.Namespace))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTwoNamespaces_ReturnsAllTypesNotInEitherExcludedNamespace()
		{
			var markerTypes = new [] { typeof(IGeneralInterface), typeof(IThreeDeepNeed) };
			var markerTypeNamespace = markerTypes.Select(type => type.Namespace).ToList();

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>()
				.ExcludeNamespaceContaining<IThreeDeepNeed>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t =>
						!markerTypeNamespace.Any(ns => t.Namespace.StartsWith(ns)))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTwoNamespaces_ReturnsNoTypesInExcludedSubNamespaces()
		{
			var markerTypes = new[] { typeof(ICompoundNeed), typeof(IGeneralInterface) };
			var markerTypeNamespaces = markerTypes.Select(type => type.Namespace).ToList();

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<ICompoundNeed>()
				.ExcludeNamespaceContaining<IGeneralInterface>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => markerTypeNamespaces.Any(ns => t.Namespace.StartsWith(ns)))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTwoTypeConditions_ReturnsNoTypesMatchingEitherCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			Func<Type, bool> matchCondition2 =
				t => t.Namespace.EndsWith("Compound");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(matchCondition)
				.ExcludeTypesWhere(matchCondition2);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => matchCondition(type) || matchCondition2(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingTwoTypeConditions_ReturnsAllTypesNotMatchingEitherCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			Func<Type, bool> matchCondition2 =
				t => t.Namespace.EndsWith("Compound");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(matchCondition)
				.ExcludeTypesWhere(matchCondition2);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !matchCondition(type) && !matchCondition2(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTwoNamespaces_ReturnsNoTypesOutsideIncludedNamespaces()
		{
			var markerTypes = new[] { typeof(IGeneralInterface), typeof(IThreeDeepNeed) };
			var markerTypeNamespaces = markerTypes.Select(type => type.Namespace).ToList();

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>()
				.IncludeNamespaceContaining<IThreeDeepNeed>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => 
						!markerTypeNamespaces.Any(ns =>
							t.Namespace.StartsWith(ns)))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTwoNamespaces_ReturnsAllTypesIncludedInEitherNamespace()
		{
			var markerTypes = new[] { typeof(IGeneralInterface), typeof(IThreeDeepNeed) };
			var markerTypeNamespaces = markerTypes.Select(type => type.Namespace).ToList();

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>()
				.IncludeNamespaceContaining<IThreeDeepNeed>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t =>
						markerTypeNamespaces.Any(ns =>
							t.Namespace.StartsWith(ns)))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTwoTypeConditions_ReturnsAllTypesMatchingEitherCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");
			
			Func<Type, bool> matchCondition2 =
				t => t.Namespace.EndsWith("Compound");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(matchCondition)
				.IncludeTypesWhere(matchCondition2);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => matchCondition(type) || matchCondition2(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingTwoTypeConditions_ReturnsNoTypesNotMatchingEitherCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> matchCondition =
				t => t.Namespace.EndsWith("ThreeDeep");

			Func<Type, bool> matchCondition2 =
				t => t.Namespace.EndsWith("Compound");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(matchCondition)
				.IncludeTypesWhere(matchCondition2);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !matchCondition(type) && !matchCondition2(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		// Exclude then include
		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneNamespace_IncludingSubNamespace_ReturnsNoTypesInExcludedNamespace()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>()
				.IncludeNamespaceContaining<SubImplementor>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => t.Namespace == typeof(IGeneralInterface).Namespace)
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneNamespace_IncludingSubNamespace_ReturnsTypesInIncludedSubNamespace()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>()
				.IncludeNamespaceContaining<SubImplementor>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => t.Namespace == typeof(SubImplementor).Namespace)
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneNamespace_IncludingSubNamespace_ReturnsAllTypesOutsideExcludedNamespace()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<IGeneralInterface>()
				.IncludeNamespaceContaining<SubImplementor>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => !t.Namespace.StartsWith(typeof(IGeneralInterface).Namespace))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneTypeCondition_IncludingOverlappingTypeCondition_ReturnsAllTypesMatchingSecondCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> exclusionCo =
				t => t.Namespace.Contains("MultipleImplementors");

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(exclusionCo)
				.IncludeTypesWhere(exclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => exclusionCondition(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneTypeCondition_IncludingOverlappingTypeCondition_ReturnsAllTypesNotMatchingExclusionCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("MultipleImplementors");

			Func<Type, bool> inclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(exclusionCondition)
				.IncludeTypesWhere(inclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !exclusionCondition(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingOneTypeCondition_IncludingOverlappingTypeCondition_ReturnsNoTypesMatchingOnlyExclusionCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("MultipleImplementors");

			Func<Type, bool> inclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.ExcludeTypesWhere(exclusionCondition)
				.IncludeTypesWhere(inclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => exclusionCondition(type) && !inclusionCondition(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		// Include then exclude

		[Fact]
		public void KnownTypes_FromAssembly_IncludingOneNamespace_ExcludingSubNamespace_ReturnsAllTypeInIncludedNamespace()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>()
				.ExcludeNamespaceContaining<SubImplementor>();

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => t.Namespace == typeof(IGeneralInterface).Namespace)
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingOneNamespace_ExcludingSubNamespace_ReturnsNoTypesInSubNamespace()
		{
			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<IGeneralInterface>()
				.ExcludeNamespaceContaining<SubImplementor>();

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(t => t.Namespace == typeof(SubImplementor).Namespace)
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingOneTypeCondition_ExcludingOverlappingTypeCondition_ReturnsAllTypesMatchingOnlyInclusionCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> inclusionCondition =
				t => t.Namespace.Contains("MultipleImplementors");

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(inclusionCondition)
				.ExcludeTypesWhere(exclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !exclusionCondition(type) && inclusionCondition(type))
					.ToList();

			foreach (var includedType in includedTypes)
			{
				Assert.Contains(includedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingOneTypeCondition_ExcludingOverlappingTypeCondition_ReturnsNoTypesMatchingExclusionCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> inclusionCondition =
				t => t.Namespace.Contains("MultipleImplementors");

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(inclusionCondition)
				.ExcludeTypesWhere(exclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => exclusionCondition(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingOneTypeCondition_ExcludingOverlappingTypeCondition_ReturnsNoTypesMatchingNeitherCondition()
		{
			var markerType = typeof(IGeneralInterface);

			var subject = new DetectTypes();

			Func<Type, bool> inclusionCondition =
				t => t.Namespace.Contains("MultipleImplementors");

			Func<Type, bool> exclusionCondition =
				t => t.Namespace.EndsWith("SubImplementor");

			subject.FromAssemblyContaining<IService>()
				.IncludeTypesWhere(inclusionCondition)
				.ExcludeTypesWhere(exclusionCondition);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				markerType.Assembly.GetTypes()
					.Where(type => !inclusionCondition(type) && !exclusionCondition(type))
					.ToList();

			foreach (var excludedType in excludedTypes)
			{
				Assert.DoesNotContain(excludedType, knownTypes);
			}
		}

		// Opinions

		[Fact]
		public void KnownTypes_FromAssembly_TakingAdviceFromOneOpinion_ReturnsAllTypesIncludedByOpinion()
		{
			var inclusions = new[] { typeof(CompoundModule), typeof(SubImplementor) };
			var exclusions = new[] { typeof(Implementor1), typeof(Implementor2) };

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.TakeAdviceFrom(
					new TypeListOpinion(
						inclusions,
						exclusions));

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var inclusion in inclusions)
			{
				Assert.Contains(inclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_TakingAdviceFromOneOpinion_ReturnsNoTypesExcludedByOpinion()
		{
			var inclusions = new[] { typeof(CompoundModule), typeof(SubImplementor) };
			var exclusions = new[] { typeof(Implementor1), typeof(Implementor2) };

			var subject = new DetectTypes();

			subject.FromAssemblyContaining<IService>()
				.TakeAdviceFrom(
					new TypeListOpinion(
						inclusions,
						exclusions));

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var exclusion in exclusions)
			{
				Assert.DoesNotContain(exclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_TypeIncludedByFirstOpinion_TypeExcludedBySecondOpinion_DoesNotReturnType()
		{
			var inclusions = new[] { typeof(CompoundModule) };
			var exclusions = new[] { typeof(CompoundModule) };

			var subject = new DetectTypes();

			var inclusionOpinion =
				new TypeListOpinion(
					inclusions,
					null);

			var exclusionOpinion =
				new TypeListOpinion(
					null,
					exclusions);

			subject.FromAssemblyContaining<IService>()
				.TakeAdviceFrom(inclusionOpinion)
				.TakeAdviceFrom(exclusionOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var exclusion in exclusions)
			{
				Assert.DoesNotContain(exclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_TypeExcludedByFirstOpinion_TypeIncludedBySecondOpinion_ReturnsType()
		{
			var inclusions = new[] { typeof(CompoundModule) };
			var exclusions = new[] { typeof(CompoundModule) };

			var subject = new DetectTypes();

			var inclusionOpinion =
				new TypeListOpinion(
					inclusions,
					null);

			var exclusionOpinion =
				new TypeListOpinion(
					null,
					exclusions);

			subject.FromAssemblyContaining<IService>()
				.TakeAdviceFrom(exclusionOpinion)
				.TakeAdviceFrom(inclusionOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var inclusion in inclusions)
			{
				Assert.Contains(inclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingNamespace_TypeInNamespaceExcludedBySecondOpinion_DoesNotReturnType()
		{
			var exclusions = new[] { typeof(CompoundModule) };

			var subject = new DetectTypes();

			var exclusionOpinion =
				new TypeListOpinion(
					null,
					exclusions);

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<CompoundModule>()
				.TakeAdviceFrom(exclusionOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var exclusion in exclusions)
			{
				Assert.DoesNotContain(exclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamepsace_TypeInNamespaceIncludedBySecondOpinion_ReturnsType()
		{
			var inclusions = new[] { typeof(CompoundModule) };

			var subject = new DetectTypes();

			var inclusionOpinion =
				new TypeListOpinion(
					inclusions,
					null);

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<CompoundModule>()
				.TakeAdviceFrom(inclusionOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			foreach (var inclusion in inclusions)
			{
				Assert.Contains(inclusion, knownTypes);
			}
		}
		
		// Indifferent opinions

		[Fact]
		public void KnownTypes_FromAssembly_IncludingNamespace_OpinionIsIndifferent_ReturnsNoTypesOutsideNamespace()
		{
			var subject = new DetectTypes();

			var indifferentOpinion =
				new TypeListOpinion(
					null,
					null);

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<ICompoundNeed>()
				.TakeAdviceFrom(indifferentOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(type =>
						!type.Namespace.StartsWith(
							typeof(ICompoundNeed).Namespace))
					.ToList();

			foreach (var exclusion in excludedTypes)
			{
				Assert.DoesNotContain(exclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_IncludingNamespace_OpinionIsIndifferent_ReturnsAllTypesInsideNamespace()
		{
			var subject = new DetectTypes();

			var indifferentOpinion =
				new TypeListOpinion(
					null,
					null);

			subject.FromAssemblyContaining<IService>()
				.IncludeNamespaceContaining<ICompoundNeed>()
				.TakeAdviceFrom(indifferentOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(type =>
						type.Namespace.StartsWith(
							typeof(ICompoundNeed).Namespace))
					.ToList();

			foreach (var inclusion in includedTypes)
			{
				Assert.Contains(inclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamespace_OpinionIsIndifferent_ReturnsAllTypesOutsideNamespace()
		{
			var subject = new DetectTypes();

			var indifferentOpinion =
				new TypeListOpinion(
					null,
					null);

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<ICompoundNeed>()
				.TakeAdviceFrom(indifferentOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			var includedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(type =>
						!type.Namespace.StartsWith(
							typeof(ICompoundNeed).Namespace))
					.ToList();

			foreach (var inclusion in includedTypes)
			{
				Assert.Contains(inclusion, knownTypes);
			}
		}

		[Fact]
		public void KnownTypes_FromAssembly_ExcludingNamespace_OpinionIsIndifferent_ReturnsNoTypesInsideNamespace()
		{
			var subject = new DetectTypes();

			var indifferentOpinion =
				new TypeListOpinion(
					null,
					null);

			subject.FromAssemblyContaining<IService>()
				.ExcludeNamespaceContaining<ICompoundNeed>()
				.TakeAdviceFrom(indifferentOpinion);

			var knownTypes = subject.KnownTypes().ToList();

			var excludedTypes =
				typeof(IService).Assembly.GetTypes()
					.Where(type =>
						type.Namespace.StartsWith(
							typeof(ICompoundNeed).Namespace))
					.ToList();

			foreach (var exclusion in excludedTypes)
			{
				Assert.DoesNotContain(exclusion, knownTypes);
			}
		}
	}
}
