using System;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Remnant.Core.Extensions;

namespace Remnant.Core.Services
{

	/// <summary>
	/// Provides Xml serialization and deserialization from and to a class using any type of encoding.
	/// </summary>
	/// <typeparam name="TEncoding">The type of encoding to be used (UnicodeEncoding (UTF-16), UTF-8, UTF-32 etc.). Ex: new UTF8Encoding()</typeparam>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Mid 2009
	/// </remarks>
	public static class XmlService<TEncoding>
		where TEncoding : Encoding, new()
	{

		#region Private Static Methods


		/// <summary>
		/// Deserialize a Xml stream to class instance
		/// </summary>
		/// <param name="stream">The xml stream</param>
		/// <returns>Returns the instance of the class</returns>
		private static object DeSerialize(Type type, Stream stream)
		{
			var xml = new XmlSerializer(type);
			return xml.Deserialize(stream);
		}

		private static object DeSerialize(Type type, XmlTextReader reader)
		{
			var xml = new XmlSerializer(type);
			return xml.Deserialize(reader);
		}

		#endregion


		#region Public Static Methods

		/// <summary>
		/// Create a class instance from the specified file
		/// </summary>
		/// <param name="fileName">Specify the full path and name of the file</param>
		/// <returns></returns>
		public static TType LoadFromFile<TType>(string fileName)
			where TType : new()
		{
			return (TType)LoadFromFile(typeof(TType), fileName);
		}

		/// <summary>
		/// Create a class instance from the specified file
		/// </summary>
		/// <param name="fileName">Specify the full path and name of the file</param>
		/// <returns></returns>
		public static object LoadFromFile(Type type, string fileName)
		{
			try
			{
				using (var reader = new XmlTextReader(fileName))
				{
					return DeSerialize(type, reader);
				}
			}
			catch (Exception e)
			{
				throw new FileLoadException(string.Format("Unable to load file '{0} from disk. ", fileName) + e.FullMessage(), e);
			}
		}

		/// <summary>
		/// Saves the instance to the specified file
		/// </summary>
		/// <param name="instance">The file instance</param>
		/// <param name="fileName">Specify the full path and name of the file</param>
		public static void SaveToFile(object instance, string fileName)
		{
			var writer = new StreamWriter(fileName, false, new TEncoding());
			try
			{
				var xml = new XmlSerializer(instance.GetType());
				xml.Serialize(writer, instance);
			}
			finally
			{
				writer.Close();
			}
		}

		/// <summary>
		/// Create a class instance from the passed xml string
		/// </summary>
		/// <returns></returns>
		public static TObjectType XmlToInstance<TObjectType>(string xml)
		{
			return (TObjectType)XmlToInstance(typeof(TObjectType), xml);
		}

		/// <summary>
		/// Create a class instance from the passed xml string
		/// </summary>
		/// <returns></returns>
		public static object XmlToInstance(Type type, string xml)
		{
			var byteArray = new TEncoding().GetBytes(xml);
			var stream = new MemoryStream(byteArray);
			try
			{
				return DeSerialize(type, stream);
			}
			finally
			{
				stream.Close();
			}
		}

		/// <summary>
		/// Static helper method to convert an object to a xml string. 
		/// </summary>
		/// <param name="instance">Instance that must be converted to a xml string.</param>
		/// <param name="namespaces">Optional, specify default namespaces (pass a blank namespace to ignore default namespaces)</param>
		/// <returns>Returns a xml string.</returns>
		public static string InstanceToXml(object instance, XmlSerializerNamespaces namespaces = null)
		{

			var serializer = new XmlSerializer(instance.GetType());

			var stream = new MemoryStream();
			var writer = new XmlTextWriter(stream, null);
			try
			{

				if (namespaces != null)
					serializer.Serialize(writer, instance, namespaces);
				else
					serializer.Serialize(writer, instance);

				Encoding encoding = new TEncoding();
				return encoding.GetString(stream.ToArray());
			}
			finally
			{
				stream.Close();
				writer.Close();
			}
		}

		#endregion

	}
}
