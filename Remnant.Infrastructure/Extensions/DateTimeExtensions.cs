using System;
using Remnant.Core.Services;

namespace Remnant.Core.Extensions
{
  public static class DateTimeExtensions
  {
    public static string SingleQuoted(this DateTime source)
    {
			return $"'{source}'";
    }
  }
}
