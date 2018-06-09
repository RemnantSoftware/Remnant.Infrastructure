using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Remnant.Core.Extensions
{
	public static class ListExtensions
	{
		public static string ToDelimitString<T>(this List<T> source, string delimiter)
		{
			if (source == null || source.Count == 0)
				return string.Empty;

			var sb = new StringBuilder();
			foreach (var item in source)
			{
				sb.Append(item);
				sb.Append(delimiter);
			}

			sb.Remove(sb.Length - 1, 1);
			return sb.ToString();
		}

		public static List<string> ToLower(this List<string> source)
		{
			source.ForEach(item =>
				{
					item = item.ToString().ToLower();
				});
			return source;
		}
	}
}
