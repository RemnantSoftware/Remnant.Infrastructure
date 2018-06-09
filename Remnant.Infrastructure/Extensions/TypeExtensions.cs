using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remnant.Core.Extensions
{
  public static class TypeExtensions
  {

    public static string TypeInfo(this Type source)
    {
    	var sb = new StringBuilder();
			if (source.IsClass)
				sb.Append("Class");
			if (source.IsInterface)
				sb.Append("Interface");
			if (source.IsEnum)
				sb.Append("Enumeration");
    	return sb.ToString();
    }    

		public static Type SuperBaseType(this Type source)
		{
			Type superType = source.BaseType;
			while (superType.BaseType != null && superType.BaseType != typeof(Object))
				superType = superType.BaseType;
			return superType;
		}
  }
}
