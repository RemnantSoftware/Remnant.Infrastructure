using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Collections;
using Remnant.Core.Services;

namespace Remnant.Core.Extensions
{
  /// <summary>
  /// Extension methods for Enumerations
  /// </summary>
  /// <remarks>
  /// Author: Neill Verreynne
  /// Date: Mid 2009
  /// </remarks>
  public static class EnumExtension
  {

    /// <summary>
    /// Retrieves the description for an enum and uses the value to replace argument in description
    /// </summary>
    /// <typeparam name="TEnum">The type of enum</typeparam>
    /// <param name="enumValue">The instance of the enum</param>
    /// <param name="value">The values to use for parsing the description</param>
    /// <returns>Returns a parsed description of the enum</returns>
    public static string ToDescription<TEnum>(this TEnum enumValue, string value) where TEnum : struct
    {
      var description = ReflectionService.GetMemberAttribute<DescriptionAttribute>(enumValue);
      return string.Format(description.Description, value);
    }

    public static string ToString<TEnum>(this TEnum enumValue, string value) where TEnum : struct
    {
      var description = ReflectionService.GetClassAttribute<DescriptionAttribute>(enumValue);
      return string.Format(description.Description, value);
    }

    /// <summary>
    /// Retrieves the description for an enum and uses a list of values to replace arguments in description
    /// </summary>
    /// <typeparam name="TEnum">The type of enum</typeparam>
    /// <param name="enumValue">The instance of the enum</param>
    /// <param name="values">The list of values to use for parsing the description</param>
    /// <returns>Returns a parsed description of the enum</returns>
    public static string ToDescription<TEnum>(this TEnum enumValue, params string[] values) where TEnum : struct
    {
      var description = ReflectionService.GetMemberAttribute<DescriptionAttribute>(enumValue);
      return string.Format(description.Description, values);
    }

		///// <summary>
		///// Retrieves the descriptions for an enum and returns them as a dictionary containing value and name
		///// </summary>
		///// <param name="enumValue">The instance of the enum</param>
		///// <returns>Returns a dictionary of values and names</returns>
		//public static Dictionary<int,string> ToDescriptionList(this Enum enumValue)
		//{
		//  var dict = new Dictionary<int, string>();
		//  var names = enumValue.ToString().Split(',');
		//  for (var index = 0; index < names.Length; index++)
		//  {
		//    var name = names[index];
		//    var value = Enum.Parse(enumValue.GetType(), name.Trim());
		//    var descriptionAttr = (ReflectionService.GetClassAttribute<DescriptionAttribute>(value));
		//    if (descriptionAttr != null)
		//      name = descriptionAttr.Description;
		//    dict.Add(index, name);
		//  }
		//  return dict;
		//}

		/// <summary>
		/// Retrieves the description for an enum
		/// </summary>
		/// <typeparam name="TEnum">The type of enum</typeparam>
		/// <param name="enumValue">The instance of the enum</param>
		/// <returns>Returns the description of the enum</returns>
		public static string ToDescription<TEnum>(this TEnum enumValue) where TEnum : struct
		{
		  var description = string.Empty;
		  var names = enumValue.ToString().Split(',');

		  if (names.Length == 1)
		  {
		    description = ReflectionService.GetMemberAttribute<DescriptionAttribute>(enumValue).Description;
		  }
		  else
		  {
		    // hanlde flags enumerations delimited by pipe char (at this stage)
		    foreach (var name in names)
		    {
		      var value = Enum.Parse(typeof(TEnum), name.Trim());
		      description += (ReflectionService.GetClassAttribute<DescriptionAttribute>(value)).Description + "| ";
		    }
		    description = description.Substring(0, description.Length - 2);
		  }

		  return description;
		}
   

    /// <summary>
    /// Extension method for generic interface IEmumerable to return the list as a delimited string.
    /// </summary>
    /// <typeparam name="T">The generic type that are contained in the list.</typeparam>
    /// <param name="source">The source</param>
    /// <param name="separator">The delimiter</param>
    /// <returns>Returns athe list as a delimited string.</returns>
    public static string ToString<T>(this IEnumerable<T> source, string separator)
    {
      if (source == null)
        throw new ArgumentException("Parameter source can not be null.");

      if (string.IsNullOrEmpty(separator))
        throw new ArgumentException("Parameter separator can not be null or empty.");

      var array = source.Where(n => n != null).Select(n => n.ToString()).ToArray();
      return string.Join(separator, array);
    }

    /// <summary>
    ///  Extension method for IEmumerable  to return the list as a delimited string.
    /// </summary>
    /// <param name="source">The source</param>
    /// <param name="separator">The delimiter</param>
    /// <returns>Returns athe list as a delimited string.</returns>
    public static string ToString(this IEnumerable source, string separator)
    {
      if (source == null)
        throw new ArgumentException("Parameter source can not be null.");

      if (string.IsNullOrEmpty(separator))
        throw new ArgumentException("Parameter separator can not be null or empty.");

      string[] array = source.Cast<object>().Where(n => n != null).Select(n => n.ToString()).ToArray();

      return string.Join(separator, array);
    }


    /// <summary>
    /// Checks if the value contains the provided  type.
    /// </summary>
    public static bool Has<T>(this System.Enum type, T value)
    {
      try
      {
        return (((int)(object)type & (int)(object)value) == (int)(object)value);
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Checks if the value is only the provided type
    /// </summary>
    public static bool Is<T>(this System.Enum type, T value)
    {
      try
      {
        return (int)(object)type == (int)(object)value;
      }
      catch
      {
        return false;
      }
    }

    /// <summary>
    /// Adds the provided value type.
    /// </summary>
    public static T Add<T>(this System.Enum type, T value)
    {
      try
      {
        return (T)(object)(((int)(object)type | (int)(object)value));
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
           string.Format(
               "Could not append value from enumerated type '{0}'.",
               typeof(T).Name
              ), ex);
      }
    }

    /// <summary>
    /// Removes the provided value type.
    /// </summary>
    public static T Remove<T>(this System.Enum type, T value)
    {
      try
      {
        return (T)(object)(((int)(object)type & ~(int)(object)value));
      }
      catch (Exception ex)
      {
        throw new ArgumentException(
                string.Format(
                   "Could not remove value from enumerated type '{0}'.",
                typeof(T).Name
                      ), ex);
      }
    }

  }
}


