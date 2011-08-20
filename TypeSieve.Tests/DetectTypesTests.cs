using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using TypeSieve.AssemblyScan;
using TypeSieve.Tests.ScannableAssembly;
using TypeSieve.Tests.ScannableAssembly.MultipleImplementors;
using TypeSieve.Tests.ScannableAssembly.Compound;

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
	}
}
