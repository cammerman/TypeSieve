using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace TypeSieve
{
	public class Namespace
	{
		private static object _namespaceLock = new object();
		private static Dictionary<Type, Namespace> _cachedNamespaces =
			new Dictionary<Type, Namespace>();

		public static Namespace Of<TMarker>()
		{
			return Namespace.Of(typeof(TMarker));
		}

		private static void CacheNamespace(Type marker)
		{
			_cachedNamespaces[marker] = 
				new Namespace(
					marker.Assembly,
					marker.Namespace,
					marker.Assembly.GetTypes()
						.Where(type => type.Namespace == marker.Namespace));
		}

		private static bool CheckCache(Type type)
		{
			return _cachedNamespaces.ContainsKey(type);
		}

		public static Namespace Of(Type marker)
		{
			if (!CheckCache(marker))
			{
				lock (_namespaceLock)
				{
					if (!CheckCache(marker))
					{
						CacheNamespace(marker);
					}
				}
			}
			
			return _cachedNamespaces[marker];
		}

		protected Namespace(Assembly assembly, string namespacePath, IEnumerable<Type> types)
		{
			Assembly = assembly;
			NamespacePath = namespacePath;
			Types = types.ToArray();
		}

		public virtual Assembly Assembly { get; private set; }
		public virtual string NamespacePath { get; private set; }
		public virtual IEnumerable<Type> Types { get; private set; }
	}
}
