#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Remnant.Core.Extensions;

namespace Remnant.Core.Services
{
	public static class MappingService
	{

		private static void SetPropertyValue(object instance, string propertyPath, object value, bool autoConstructAggregates)
		{
			PropertyInfo propInfo = null;
			var splitPath = propertyPath.Split('.');

			for (var index = 0; index < splitPath.Length; index++)
			{
				var propName = splitPath[index];
				var listDetected = false;
				var listIndex = -1;

				// a list specified
				var expr = new Regex(@"\w+\[\d*\]");
				if (expr.IsMatch(propName))
				{
					listDetected = true;
					listIndex = Convert.ToInt32(propName.SubstringBetween("[", "]"));
					propName = propName.Substring(0, propName.IndexOf('['));
				}

				propInfo = instance.GetType().GetProperty(propName,
											BindingFlags.SetProperty |
											BindingFlags.IgnoreCase |
											BindingFlags.Public |
											BindingFlags.Instance);

				if (propertyPath.Contains('.') && index < splitPath.Length - 1)
				{
					var currInstance = instance;
					instance = propInfo.GetValue(currInstance, null);
					if (instance == null && autoConstructAggregates)
					{
						instance = DomainsService.Instance.CreateInstance(propInfo.PropertyType);
						propInfo.SetValue(currInstance, instance, null);
					}
					if (listDetected && instance != null)
					{
						var list = instance as IList;
						if (listIndex == list.Count)
						{
							var listItemType = propInfo.PropertyType.GetGenericArguments()[0];
							instance = DomainsService.Instance.CreateInstance(listItemType);
							list.Add(instance);
						}
						else
						{
							Shield.Against(listIndex > list.Count)
								.WithMessage("You have defined an index [] higher than the current count in the list!!")
								.Raise();
							instance = list[listIndex];
						}
					}
					else
						// ignore due to auto construct off etc.
						if (instance == null)
							return;
				}
				else
				{
					if (propInfo == null && listDetected)
					{
						var list = instance as IList;
						if (listIndex == list.Count)
						{
							var listItemType = instance.GetType().GetGenericArguments()[0];
							instance = DomainsService.Instance.CreateInstance(listItemType);
							list.Add(instance);
						}
						else
							instance = list[listIndex];

						propInfo = instance.GetType().GetProperty(propName);
					}
				}
			}
			if (propInfo != null && instance != null)
			{
				SetPropertyValue(instance, propInfo, value);
			}
		}

		private static void SetPropertyValue(object instance, PropertyInfo property, object value)
		{
			object setValue;

			if (property.PropertyType.IsEnum)
			{
				setValue = Enum.Parse(property.PropertyType, value.ToString(), true);
			}
			else
				if (property.PropertyType.Name.StartsWith("Nullable`"))
				{
					var underlyingType = Nullable.GetUnderlyingType(property.PropertyType);
					setValue = Convert.ChangeType(value, underlyingType);
				}
				else
				{
					var converter = TypeDescriptor.GetConverter(property.PropertyType);
					setValue = converter.CanConvertFrom(value.GetType()) ? converter.ConvertFrom(value) : value;

					//setValue = Convert.ChangeType(value, property.PropertyType);
				}
			property.SetValue(instance, setValue, null);
		}


		public static object MapToType(NameValueCollection source, Type targetType, bool autoConstructAggregates = false)
		{
			var instance = DomainsService.Instance.CreateInstance(targetType);

			foreach (var key in source.AllKeys)
			{
				if (!string.IsNullOrEmpty(source[key]) && source[key] != "null")
					SetPropertyValue(instance, key, source[key], autoConstructAggregates);
			}

			return instance;
		}
	}
}
#endif