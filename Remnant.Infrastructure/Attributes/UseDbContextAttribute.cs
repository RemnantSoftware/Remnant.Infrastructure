using System;

namespace Remnant.Core.Attributes
{
	 [AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
  public class UseDbContextAttribute : Attribute
  {   
  }
}
