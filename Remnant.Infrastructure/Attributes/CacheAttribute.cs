using System;

namespace Remnant.Core.Attributes
{
  [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
  public class CacheAttribute : Attribute
  {
		public CacheAttribute()
		{
			Sharable = true;
			Timeout = Timeout.Indefinite;
			Singleton = false;
		}

    public Timeout Timeout { get; set; }
		public int? CustomTimeout { get; set; }
		public bool Sharable { get; set; }
		public bool Singleton { get; set; }
  }
}
