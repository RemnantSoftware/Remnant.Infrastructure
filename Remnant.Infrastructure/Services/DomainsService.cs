#if (!NETCOREAPP1_0 && !NETCOREAPP1_1 && !NETCOREAPP2_0 && !NETCOREAPP2_1)
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using Remnant.Core.Creational;

namespace Remnant.Core.Services
{

	/// <summary>
	/// The Domains Manager manages domains, especially for constructing types
	/// </summary>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Mid 2009
	/// </remarks>
	public sealed class DomainsService : Singleton<DomainsService>
	{
		#region Fields

		private readonly IDictionary<string, Type> _types = new Dictionary<string, Type>();
		private readonly IDictionary<string, AppDomain> _domains = new Dictionary<string, AppDomain>();
		private readonly List<string> _registerTypesStartingWith = new List<string>();

		#endregion

		#region Constructors & Finalizors

		public DomainsService()
			: base()
		{
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Retrieve all the assembly types that are loaded in a domain
		/// </summary>
		/// <param name="domain">The targeted domain</param>
		private void GetDomainAssemblyTypes(AppDomain domain)
		{
			var assemblies = domain.GetAssemblies();
			foreach (var assembly in assemblies)
			{
				RegisterTypes(assembly);
				if (OnAssemblyLoaded != null)
					OnAssemblyLoaded(assembly);
			}
		}

		/// <summary>
		/// This event will be called when an assembly cannot be resolved
		/// </summary>
		/// <param name="sender">The sender who raised the event</param>
		/// <param name="args">The resolved event arguments</param>
		/// <returns>Returns the loaded assembly</returns>
		private static Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			//if (!args.Name.Contains("Xml")) // ignore message for dynamically generated xml assemblies by the framework etc.
			//Debug.WriteLine("DomainsManager: Unable to resolve assembly: " + args.Name);
			return null;
		}

		/// <summary>
		/// Register the types of an assembly that only starts with specified qualifiers, with the domains manager
		/// Note: if none startswith specified, register all
		/// </summary>
		/// <param name="assembly">The targeted assembly</param>
		private void RegisterTypes(Assembly assembly)
		{
			var mustRegister = _registerTypesStartingWith.Count == 0 ||
				(from entry in _registerTypesStartingWith
				 where assembly.FullName.StartsWith(entry, StringComparison.InvariantCultureIgnoreCase)
				 select true).Distinct().SingleOrDefault();

			// register types for loaded assemblies that does not start with "System"
			if (mustRegister)
			{
				//Console.WriteLine(@"!!! DomainsManager: Registering types for assembly " + assembly.FullName);
				try
				{
					var assemblyTypes = assembly.GetTypes();
					foreach (var type in assemblyTypes)
					{
						if (!_types.ContainsKey(type.FullName))
							_types.Add(type.FullName, type);
					}
				}
				catch (Exception e)
				{
					throw e;
				}
			}
		}

		/// <summary>
		/// This event is called when an assembly is loaded into the current domain.
		/// It will register all the types of the assembly with the domains manager.
		/// </summary>
		/// <param name="sender">The sender who raised the event</param>
		/// <param name="args">The event arguments of the assembly that is loaded</param>
		private void Domain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
		{
			RegisterTypes(args.LoadedAssembly);
			//Debug.WriteLine(args.LoadedAssembly.FullName);
			if (OnAssemblyLoaded != null)
				OnAssemblyLoaded(args.LoadedAssembly);
		}

		/// <summary>
		/// Creates a generic class type definition.
		/// </summary>
		/// <param name="typeName">The typename including the generic type arguments.</param>
		/// <returns>Returns the created instance.</returns>
		private static object CreateGenericInstance(string typeName)
		{
			// obtain the generic type arguments
			var genArgsSplit = new string[0];
			var startPos = typeName.IndexOf('<') + 1;
			var genArgs = typeName.Substring(startPos, typeName.Length - startPos - 1);
			if (genArgs.Contains(','))
				genArgsSplit = genArgs.Split(',');
			else
				genArgsSplit[0] = genArgs;

			var genTypeName = typeName.Substring(0, startPos - 1);
			return ReflectionService.CreateGenericType(genTypeName, genArgsSplit);
		}

		#endregion

		#region Public Members

		/// <summary>
		/// Add a domain to the domains manager.
		/// </summary>
		/// <param name="domain">The domain that must be added</param>
		public DomainsService AddDomain(AppDomain domain)
		{
			if (!_domains.Contains(new KeyValuePair<string, AppDomain>(domain.FriendlyName, domain)))
			{
				domain.AssemblyLoad += Domain_AssemblyLoad;
				domain.AssemblyResolve += Domain_AssemblyResolve;
				domain.UnhandledException += Domain_UnhandledException;
				_domains.Add(domain.FriendlyName, domain);
				GetDomainAssemblyTypes(domain);
			}
			return this;
		}

		private void Domain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (OnUnhandledException != null)
				OnUnhandledException(sender, e);
		}

		private static bool IsAssemblyLoaded(string fileName)
		{
			var name = IOService.ExtractFileNameWithExtension(fileName);
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.ManifestModule.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) != null;
		}

		/// <summary>
		/// Creates a new domain and adds it to the domain manager
		/// </summary>
		/// <param name="domainName">The name of the domain</param>
		/// <returns></returns>
		public AppDomain CreateDomain(string domainName)
		{
			var domain = AppDomain.CreateDomain(domainName);
			AddDomain(domain);
			return domain;
		}

		/// <summary>
		/// Unloads a domain from memory and domains manager
		/// </summary>
		/// <param name="domainName"></param>
		public void UnloadDomain(string domainName)
		{
			AppDomain domain = null;
			_domains.TryGetValue(domainName, out domain);
			_domains.Remove(domainName);
			// TODO: remove all registered types
			AppDomain.Unload(domain);
		}


		/// <summary>
		/// Register the types of an assembly that only starts with specified qualifiers
		/// </summary>
		/// <param name="startsWith"></param>
		/// <returns></returns>
		public DomainsService RegisterAssemblyTypesStartingWith(string startsWith)
		{
			if (!_registerTypesStartingWith.Contains(startsWith))
				_registerTypesStartingWith.Add(startsWith);
			return this;
		}

		/// <summary>
		/// Loads an assembly from a file
		/// </summary>
		/// <param name="fileName">The full path and filename of the assembly</param>
		/// <returns></returns>
		public Assembly LoadAssembly(string fileName)
		{
			if (!fileName.EndsWith(".dll", true, CultureInfo.CurrentCulture))
				fileName += ".dll";

			Shield.Against(!IOService.FileExists(fileName))
				.WithMessage("The file cannot be found : " + fileName)
				.Raise();

			if (IsAssemblyLoaded(fileName))
			{
				var name = IOService.ExtractFileNameWithExtension(fileName);
				return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.ManifestModule.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			}

			return Assembly.LoadFile(fileName);
		}

		/// <summary>
		/// Loads an assembly from a file using current domain path
		/// Note: Loading is ignored if already loaded in currnt domain (and is exactly the same path file)
		/// </summary>
		/// <param name="fileName">The filename of the assembly</param>
		/// <returns></returns>
		public Assembly LoadAssemblyUsingDomainPath(string fileName)
		{
			
			var codeBase = Assembly.GetExecutingAssembly().CodeBase;
			var uri = new UriBuilder(codeBase);
			var path = Uri.UnescapeDataString(uri.Path);

			var fullPath = Path.Combine(Path.GetDirectoryName(path), fileName);

			return LoadAssembly(fullPath);
		}

		/// <summary>
		/// Loads an assembly by providing the long form of its name
		/// </summary>
		/// <param name="fullName">The long form name of the assembly</param>
		/// <returns></returns>
		public Assembly LoadAssemblyLongName(string fullName)
		{
			return Assembly.Load(fullName);
		}

		/// <summary>
		/// Asks the domains manager to provide the Type for a type name
		/// </summary>
		/// <param name="typeName">The name of the type</param>
		/// <returns></returns>
		public System.Type GetTypeFromName(string typeName)
		{
			try
			{
				return _types[typeName];
			}
			catch (Exception e)
			{
				throw new Exception("DomainsManager: does not recognize type of " + typeName + ". " + e.Message + " Unable to resolve the type. Ensure that the assembly was referenced or make sure your spelling is correct.", e.InnerException);
			}
		}

		/// <summary>
		/// Asks the domain manager to return an array of Types for an array of type names
		/// </summary>
		/// <param name="typeNames">An array of type names</param>
		/// <returns>Rteurns an array of Types</returns>
		public Type[] GetTypesFromNames(string[] typeNames)
		{
			return typeNames.Select(typeName => GetTypeFromName(typeName.Trim())).ToArray();
		}

		public List<Type> GetClassedDerivedFrom(Type baseClassType)
		{
			Shield.Against(!baseClassType.IsClass)
				.WithMessage("DomainsService: Argument baseClassType is not of a class type")
				.Raise<ArgumentException>();

			var foundClasses = (from type in _types.Values
													where type.IsClass && type.BaseType == baseClassType
													select type);

			return foundClasses.ToList();
		}

		/// <summary>
		/// Instantiate an object from a Type (doesn't handle generic types).
		/// </summary>
		/// <param name="type">The Type that must be used to create an instance from</param>
		/// <param name="args">An array of parameters that will be passed to the constructor</param>
		/// <returns>Returns an instantiated object</returns>
		public object CreateInstance(Type type, params object[] args)
		{
			return Activator.CreateInstance(type, args);
		}

		/// <summary>
		/// Instantiate an object from a Type (doesn't handle generic types).
		/// </summary>
		/// <param name="args">An array of parameters that will be passed to the constructor</param>
		/// <returns>Returns an instantiated object</returns>
		public TType CreateInstance<TType>(params object[] args)
		{
			return (TType)Activator.CreateInstance(typeof(TType), args);
		}

		/// <summary>
		/// Instantiate an object from a name of a type (handles generic types as well).
		/// </summary>
		/// <param name="typeName">The name of the type</param>
		/// <returns>Returns an instantiated object</returns>
		public object CreateInstance(string typeName)
		{
			return CreateInstance(typeName, null);
		}

		/// <summary>
		/// Instantiate an object from a name of a type with parameters (handles generic types as well).
		/// </summary>
		/// <param name="typeName">The name of the type</param>
		/// <param name="args">An array of parameters that will be passed to the constructor</param>
		/// <returns>Returns an instantiated object</returns>
		public object CreateInstance(string typeName, params object[] args)
		{
			// check if a generic type must be constructed
			if (typeName.Contains('<') && typeName.Contains('>'))
			{
				return CreateGenericInstance(typeName);
			}

			var type = GetTypeFromName(typeName);
			if (type == null)
				throw new TypeLoadException("Domain manager: The following type cannot be found '" + typeName + "'");
			var instance = Activator.CreateInstance(type, args);
			return instance;
		}

		public IDictionary<string, Type> AllTypes { get { return _types; } }

		#endregion

		#region Events

		/// <summary>
		/// Trap unhandled exceptions from all threads within appdomain
		/// </summary>
		public event Action<object, UnhandledExceptionEventArgs> OnUnhandledException;

		public event Action<Assembly> OnAssemblyLoaded;

		#endregion
	}
}
#endif