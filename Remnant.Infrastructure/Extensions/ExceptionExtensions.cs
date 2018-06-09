using System;
using System.Reflection;
using System.Text;

namespace Remnant.Core.Extensions
{
  public static class ExceptionExtensions
  {

    public static string FullMessage(this Exception source)
    {
      var sb = new StringBuilder();
      var recurse = source is TargetInvocationException 
				? source.InnerException ?? source
				: source; 

      while (recurse != null)
      {
        sb.Append(recurse.Message);
        recurse = recurse.InnerException;
      }
			return sb.ToString();
    }

  }
}
