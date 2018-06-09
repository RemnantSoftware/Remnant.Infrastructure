using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remnant.Core.Extensions
{
  public static class ObjectExtensions
  {

    public static TType ToType<TType>(this object source)
    {
      return (TType)Convert.ChangeType(source, typeof (TType));
    }    
  }
}
