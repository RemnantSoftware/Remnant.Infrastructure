using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Remnant.Core.Services;

namespace Remnant.Core.Extensions
{
	public static class StringExtensions
	{
		public static bool IsNumeric(this string source)
		{
			return Int32.TryParse(source, out int numeric);
		}

		public static bool IsNumeric64(this string source)
		{
			return Int64.TryParse(source, out Int64 numeric);
		}

		public static int ToInt32(this string source)
		{
			return Convert.ToInt32(source);
		}

		public static string ToCase(this string source, Case toCase)
		{
			switch (toCase)
			{
				case Case.Default:
					return source;
				case Case.Lower:
					return source.ToLower();
				case Case.Upper:
					return source.ToUpper();
				case Case.Pascal:
					return string.IsNullOrEmpty(source)
						? source
						: source.Replace(source[0].ToString(), source[0].ToString().ToUpper());
				case Case.Camel:
					return string.IsNullOrEmpty(source)
						? source
						: source.Replace(source[0].ToString(), source[0].ToString().ToLower());
				default:
					throw new NotImplementedException("String extension 'ToCase' feature not yet implemented.");
			}
		}

		public static string SurroundWith(this string source, string prefix, string suffix = null)
		{
			suffix = suffix ?? prefix;
			return prefix + source + suffix;
		}

		public static string SurroundWithSpace(this string source)
		{
			return SurroundWith(source, " ", " ");
		}

		public static string ToDoubleQuoted(this string source)
		{
			return SurroundWith(source, "\"", "\"");
		}

		public static string ToSingleQuoted(this string source)
		{
			return SurroundWith(source, "'", "'");
		}
		public static string ToBracketQuoted(this string source)
		{
			return SurroundWith(source, "(", ")");
		}

		public static string SubstringBetween(this string source, string from, string to)
		{
			var fromIndex = source.IndexOf(from) + from.Length;
			var toIndex = source.IndexOf(to, fromIndex);
			return source.Substring(fromIndex, toIndex - fromIndex);
		}

		public static string SurroundSubstring(this string source, string target, string prefix, string suffix, StringComparison comparison)
		{
			var index = source.IndexOf(target, comparison);

			// if match found
			if (index >= 0)
			{
				source = source.Insert(index, prefix);
				source = source.Insert(index + prefix.Length + target.Length, suffix);
			}

			return source;
		}

		public static int CountPlaceHolders(this string source)
		{
			return Regex.Matches(source.Replace("{{", string.Empty), @"\{(\d+)")
					.OfType<Match>()
					.Select(match => int.Parse(match.Groups[1].Value))
					.Union(Enumerable.Repeat(-1, 1))
					.Max() + 1;
		}

		public static bool Match(this string source, string target, string[] ignoreChars)
		{
			if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target))
				return true;

			var tempSource = source;
			var tempTarget = target;

			foreach (var ignoreChar in ignoreChars)
			{
				if (tempSource != null)
					tempSource = tempSource.Replace(ignoreChar, string.Empty);
				if (tempTarget != null)
					tempTarget = tempTarget.Replace(ignoreChar, string.Empty);
			}
			return tempSource.Equals(tempTarget);
		}

		public static bool Match(this string source, string target, string[] ignoreChars, StringComparison stringComparison)
		{
			foreach (var ignoreChar in ignoreChars)
			{
				source = source.Replace(ignoreChar, string.Empty);
				target = target.Replace(ignoreChar, string.Empty);
			}
			return source.Equals(target, stringComparison);
		}

		public static string SplitOnLength(this string input, int length)
		{
			var index = 0;
			var output = new StringBuilder();

			while (index < input.Length)
			{
				output.AppendLine(index + length < input.Length
					? input.Substring(index, length)
					: input.Substring(index));

				index += length;
			}
			return output.ToString();
		}

		//public static Regex RegEx(this string source, string expression)
		//{
		//  return new Regex(expression);
		//}
	}
}
