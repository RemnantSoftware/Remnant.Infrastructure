using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace Remnant.Core.Services
{
	public static class ResourceService
	{
		public static Dictionary<string, T> GetAssemblyResources<T>(Assembly assembly, string baseName, bool ignoreExceptions, params string[] names)
		{
			var list = new Dictionary<string, T>();
			var resMgr = new ResourceManager(assembly.GetName().Name + baseName, assembly);

			foreach (var name in names)
			{
				try
				{
					var t = (T)resMgr.GetObject(name);
					list.Add(name,t);
				}
				catch (Exception)
				{
					if (!ignoreExceptions)
						throw;
				}
			}
			return list;
		}
	}
}
