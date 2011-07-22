using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeSieve.Tests.ScannableAssembly.Compound
{
	public class CompoundModule : ICompoundNeed
	{
		public CompoundModule(IService service, INeed need)
		{
			if (need == null)
				throw new ArgumentNullException("need");

			if (service == null)
				throw new ArgumentNullException("service");
		}
	}
}