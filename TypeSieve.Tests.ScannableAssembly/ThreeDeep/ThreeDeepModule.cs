using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TypeSieve.Tests.ScannableAssembly.Compound;

namespace TypeSieve.Tests.ScannableAssembly.ThreeDeep
{
    public class ThreeDeepModule : IThreeDeepNeed
    {
        public ThreeDeepModule(ICompoundNeed compound)
        {
            if (compound == null)
                throw new ArgumentNullException("compound");
        }
    }
}