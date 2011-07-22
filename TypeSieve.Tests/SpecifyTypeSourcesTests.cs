using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using TypeSieve.AssemblyScan;
using TypeSieve.Tests.ScannableAssembly;

namespace TypeSieve.Tests
{
    public class SpecifyTypeSourcesTests
    {
        [Fact]
        public void FromAssemblyContaining_T_StoresAssembly()
        {
            var subject = new DetectTypes();

            subject.FromAssemblyContaining<IService>();

            Assert.Contains(
                typeof(IService).Assembly,
                subject.SourceAssemblies);
        }

        [Fact]
        public void FromAssemblyContaining_T_ReturnsSelfAsFilter()
        {
			var subject = new DetectTypes();

            Assert.Same(
                subject,
                subject.FromAssemblyContaining<IService>());
        }
    }
}
