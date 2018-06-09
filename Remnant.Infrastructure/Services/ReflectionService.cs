using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using Remnant.Core.Attributes;

namespace Remnant.Core.Services
{
  /// <summary>
  /// The Reflection Helper assists with reflection routines
  /// </summary>
  /// <remarks>
  /// Author: Neill Verreynne
  /// Date: Mid 2009
  /// </remarks>
  public static class ReflectionService
  {
    private const string MsgPropertyNotFound = "Property '{0}' not found, unable to set value on instance '{1}'.";

#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
		/// <summary>
		/// Instantiate a generic object from a type name.
		/// </summary>
		/// <param name="type">The name of the type.</param>
		/// <param name="args">The parameters for the object</param>
		/// <param name="genericTypeArgs">The generic type arguments.</param>
		/// <returns>Returns the instantiated object.</returns>
		public static object CreateGenericType(string type, object[] args, params string[] genericTypeArgs)
    {
      var genType = DomainsService.Instance.GetTypeFromName(type).GetGenericTypeDefinition();
      var genArgs = DomainsService.Instance.GetTypesFromNames(genericTypeArgs);
      return CreateGenericType(genType, args, genArgs);
    }
#endif

		/// <summary>
		/// Instantiate a generic object from a type.
		/// </summary>
		/// <param name="genericType">The generic class type definition.</param>
		/// <param name="args">The parameters for the object</param>
		/// <param name="genericTypeArgs">The generic type arguments.</param>
		/// <returns>Returns the instantiated object.</returns>
		public static object CreateGenericType(Type genericType, object[] args, params Type[] genericTypeArgs)
    {
      var construct = genericType.MakeGenericType(genericTypeArgs);
      return Activator.CreateInstance(construct, args);
    }

    /// <summary>
    /// Set the value of a property of an instance using reflection.
    /// Note: Will look for public and non public properties
    /// </summary>
    /// <param name="instance">The targeted instance</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="value">The value that the property will be set to</param>
    /// <param name="ignoreIfNotFound">Ignore if property is not found on the instance, otherwise throw an exception</param>
    public static void SetPropertyValue(object instance, string propertyName, object value, bool ignoreIfNotFound)
    {
			PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      if (propertyInfo != null)
        propertyInfo.SetValue(instance, value, null);
      else
        if (!ignoreIfNotFound)
          throw new Exception(string.Format(MsgPropertyNotFound, propertyName, instance));
    }

    /// <summary>
    /// Get the value of a property of an instance using reflection
		/// Note: Will look for public and non public properties
    /// </summary>
    /// <param name="instance">The targeted instance</param>
    /// <param name="propertyName">The name of the property</param>
    /// <param name="ignoreIfNotFound">Ignore if property is not found on the instance, otherwise throw an exception</param>
    /// <returns>Returns the value of the property or null if not found if 'ignoreNotFound' is set to true.</returns>
    public static object GetPropertyValue(object instance, string propertyName, bool ignoreIfNotFound)
    {
			PropertyInfo propertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      if (propertyInfo != null)
      {
        var value = propertyInfo.GetValue(instance, null);
        return propertyInfo.PropertyType.IsEnum ? GetMemberAttribute<DescriptionAttribute>(value).Description : value;
      }
      if (!ignoreIfNotFound)
        throw new Exception(string.Format(MsgPropertyNotFound, propertyName, instance));
      
      return null;
    }
		
		public static PropertyInfo GetProperty(object instance, string propertyName, bool ignoreIfNotFound)
		{
			var propertyInfo = instance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (propertyInfo == null && !ignoreIfNotFound)
				throw new Exception(string.Format(MsgPropertyNotFound, propertyName, instance));
			return propertyInfo;
		}

    /// <summary>
    /// Retrieve a collection of methods that contain a specific attribute declaration.
    /// </summary>
    /// <param name="instance">The instance on which methods will be searched on.</param>
    /// <param name="attributeType">The type of attribute that is searched for.</param>
    /// <returns>Returns a list of found methods.</returns>
    public static IEnumerable<MethodInfo> GetMethodsWithAttribute(object instance, Type attributeType)
    {
      MethodInfo[] allMethods = instance.GetType().GetMethods();
      var methods = allMethods.Where(method => method.GetCustomAttributes(attributeType, true).Length == 1);
      return methods;
    }


		/// <summary>
		/// Retrieve a public method
		/// </summary>
		/// <param name="instance">The instance on which methods will be searched on.</param>
		/// <param name="methodName">The name of the method (case is ignored)</param>
		/// <param name="types">Optional list of parameter types required for specific method signature</param>
		/// <returns>The method</returns>
		public static MethodInfo GetMethod(object instance, string methodName, params Type[] types)
		{
			return types.Length == 0 
				? instance.GetType().GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
				: instance.GetType().GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance, null, types, null);
		}

    /// <summary>
    /// Gets a class type that implements the specified interface type in an assembly.
    /// Note: It will retrieve the first class in the assembly that implements the interface
    /// </summary>
    /// <param name="assembly">The assembly's types that will be searched</param>
    /// <param name="interfaceType">The interface type that the class implements</param>
    /// <returns></returns>
    public static Type GetClassThatImplementsInterface(Assembly assembly, Type interfaceType)
    {
      if (!interfaceType.IsInterface)
        throw new ArgumentException("ReflectionHelper: Argument is not of an interface type.", "interfaceType");
      var classType = (from type in assembly.GetTypes()
                       where type.IsClass
                       from infType in type.GetInterfaces()
                       where infType == interfaceType
                       select type).SingleOrDefault();
      return classType;
    }


		/// <summary>
		/// Gets a classes that are derived DIRECTLY from a base class in an assembly.
		/// </summary>
		/// <param name="assembly">The assembly's types that will be searched</param>
		/// <param name="baseClassType">The interface type that the class implements</param>
		/// <returns></returns>
		public static List<Type> GetClassedDerivedFrom(Assembly assembly, Type baseClassType)
		{
			if (!baseClassType.IsClass)
				throw new ArgumentException("ReflectionHelper: Argument is not of a class type.", "baseClassType");
			var foundClasses = (from type in assembly.GetTypes()
											 where type.IsClass && type.BaseType == baseClassType
											 select type);
			return foundClasses.ToList();
		}

    /// <summary>
    /// Searches for a property in the given property path
    /// </summary>
    /// <param name="rootType">The root/starting point to start searching</param>
    /// <param name="propertyPath">The path to the property. Ex Customer.Address.City</param>
    /// <returns>A <see cref="PropertyInfo"/> describing the property in the property path.</returns>
    public static PropertyInfo GetPropertyFromPath(Type rootType, string propertyPath)
    {
      if (rootType == null)
        throw new ArgumentNullException("rootType");

      Type propertyType = rootType;
      PropertyInfo propertyInfo = null;
      string[] pathElements = propertyPath.Split(new char[1] { '.' });
      foreach (string t in pathElements)
      {
      	propertyInfo = propertyType.GetProperty(t, BindingFlags.Public | BindingFlags.Instance);
      	if (propertyInfo != null)
      		propertyType = propertyInfo.PropertyType;
      	else
      		throw new ArgumentOutOfRangeException("propertyPath", propertyPath, "Invalid property path");
      }
      return propertyInfo;
    }

    public static Version GetExecutingAssemblyVersion()
    {
      return Assembly.GetExecutingAssembly().GetName().Version;
    }

    public static PropertyDescriptor GetPropertyDescriptorFromPath(Type rootType, string propertyPath)
    {
      string propertyName;
      bool lastProperty = false;
      if (rootType == null)
        throw new ArgumentNullException("rootType");

      if (propertyPath.Contains("."))
        propertyName = propertyPath.Substring(0, propertyPath.IndexOf("."));
      else
      {
        propertyName = propertyPath;
        lastProperty = true;
      }

      PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(rootType)[propertyName];
      if (propertyDescriptor == null)
        throw new ArgumentOutOfRangeException("propertyPath", propertyPath, string.Format("Invalid property path for type '{0}' ", rootType.Name));


      if (!lastProperty)
        return GetPropertyDescriptorFromPath(propertyDescriptor.PropertyType, propertyPath.Substring(propertyPath.IndexOf(".") + 1));
      else
        return propertyDescriptor;
    }

    public static TClass LazyInitGeneric<TClass>(ref TClass instance, Type[] genericTypes)
      where TClass : new()
    {
      if (instance == null)
        if (typeof(TClass).IsGenericType)
          instance = (TClass)CreateGenericType(typeof(TClass), genericTypes);
        else
          throw new ArgumentException("The class specified for generic lazy instantiation is not of generic type.");
      return instance;
    }

    public static TClass LazyInit<TClass>(ref TClass instance)
      where TClass : new()
    {
      if (instance == null)
        instance = new TClass();
      return instance;
    }

    public static TAttribute GetClassAttribute<TAttribute>(object instance) where TAttribute : Attribute
    {
    	return GetClassAttribute<TAttribute>(instance.GetType());
    }

		public static IEnumerable<TAttribute> GetClassAttributes<TAttribute>(Type type) where TAttribute : Attribute
		{
			var allAttrs = type.GetCustomAttributes(typeof(TAttribute), true);
			var attrs = (from attribute in allAttrs
				where attribute.GetType() == typeof (TAttribute)
				select attribute as TAttribute);

			return attrs;
		}

		public static TAttribute GetClassAttribute<TAttribute>(Type type) where TAttribute : Attribute
		{
			var attrs = type.GetCustomAttributes(typeof(TAttribute), true);
			var attr = (from attribute in attrs
									where attribute.GetType() == typeof(TAttribute)
									select attribute).SingleOrDefault();

			return attr as TAttribute;
		}

		public static TAttribute GetMethodAttribute<TAttribute>(MethodInfo method) where TAttribute : Attribute
		{
			var attrs = method.GetCustomAttributes(typeof(TAttribute), true);
			var attr = (from attribute in attrs
									where attribute.GetType() == typeof(TAttribute)
									select attribute).SingleOrDefault();

			return attr as TAttribute;
		}

    public static TAttribute GetMemberAttribute<TAttribute>(object instance) 
			where TAttribute : Attribute
    {
      var memberInfo = instance.GetType().GetMember(instance.ToString());
      var attrs = memberInfo[0].GetCustomAttributes(typeof(TAttribute), true);
      if (attrs.Length == 0)
        throw new Exception("Unable to retrieve custom attribute on member type!");
      return attrs[0] as TAttribute;

      //var type = enumVal.GetType();
      //var memInfo = type.GetMember(enumVal.ToString());
      //var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
      //return (T)attributes[0];
    }

    public static TAttribute GetPropertyAttribute<TAttribute>(object instance) 
			where TAttribute : Attribute
    {
	    return GetPropertyAttribute<TAttribute>(instance.GetType());
    }

		public static TAttribute GetPropertyAttribute<TAttribute>(Type type)
		 where TAttribute : Attribute
		{
			var props = type.GetProperties();
			var attr = (from p in props
									from a in p.GetCustomAttributes(true)
									where a.GetType() == typeof(TAttribute)
									select a).SingleOrDefault();
			return attr as TAttribute;
		}

		public static Dictionary<Type, List<TAttribute>> GetClassesUsingAttribute<TAttribute>(Assembly assembly) 
			where TAttribute : Attribute
		{
			return GetClassesUsingAttribute<TAttribute>(assembly.GetTypes());
		}

		public static Dictionary<Type, List<TAttribute>> GetClassesUsingAttribute<TAttribute>(Type[] types)
			where TAttribute : Attribute
		{
			var result = new Dictionary<Type, List<TAttribute>>();
			var classes = (from c in types
										 from attr in c.GetCustomAttributes(true)
										 where c.IsClass && attr.GetType() == typeof(TAttribute)
										 select new { ClassType = c, Attribute = attr });

			foreach (var @class in classes)
			{
				if (result.ContainsKey(@class.ClassType))
					result[@class.ClassType].Add((TAttribute)@class.Attribute);
				else
					result.Add(@class.ClassType, new List<TAttribute>() { (TAttribute)@class.Attribute });
			}
			return result;
		}

    public static Dictionary<string,TAttribute> GetPropertiesUsingAttribute<TAttribute>(object instance) 
			where TAttribute : Attribute
    {
      var props = instance.GetType().GetProperties();
      var list = (from p in props
                  from attribute in p.GetCustomAttributes(true)
                  where attribute.GetType() == typeof(TAttribute)
                  select new {p.Name, attribute});
      return list.ToDictionary(item => item.Name, item => (TAttribute)item.attribute);
    }

  	public static Dictionary<string, TAttribute> GetPropertiesAttribute<TAttribute>(Type type) 
			where TAttribute : BlueMercsAttribute
		{
			var props = type.GetProperties();
			var list = (from p in props
									from attribute in p.GetCustomAttributes(true)
									where attribute.GetType() == typeof(TAttribute)
									orderby ((TAttribute)attribute).Sequence
									select new { p.Name, attribute });
			return list.ToDictionary(item => item.Name, item => (TAttribute)item.attribute);
		}

		/// <summary>
		/// Get a property using an attribute
		/// </summary>
		/// <typeparam name="TAttribute">The attribute type</typeparam>
		/// <param name="instance">The instance</param>
		/// <returns>Return the property info or null if not found</returns>
    public static PropertyInfo GetPropertyUsingAttribute<TAttribute>(object instance) 
			where TAttribute : Attribute
    {
	    return GetPropertyUsingAttribute<TAttribute>(instance.GetType());
    }

		/// <summary>
		/// Get a property using an attribute
		/// </summary>
		/// <typeparam name="TAttribute">The attribute type</typeparam>
		/// <param name="type">The type</param>
		/// <returns>Return the property info or null if not found</returns>
		public static PropertyInfo GetPropertyUsingAttribute<TAttribute>(Type type)
			where TAttribute : Attribute
		{
			var props = type.GetProperties();
			var prop = (from p in props
									from a in p.GetCustomAttributes(true)
									where a.GetType() == typeof(TAttribute)
									select p).SingleOrDefault();
			return prop;
		}

  }
}