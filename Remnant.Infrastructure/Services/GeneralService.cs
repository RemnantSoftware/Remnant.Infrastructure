using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

using System.Text.RegularExpressions;

namespace Remnant.Core.Services
{
	/// <summary>
	/// The General Helper assists with common routines.
	/// </summary>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Mid 2009
	/// </remarks>
	public static class GeneralService
	{
		#region Commented out
		/*
    /// <summary>
    /// Serializes an instance to xml using UTF8 encoding and the standard XmlSerializer.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="instance">The instance to be serialized.</param>
    /// <returns>Returns a xml string.</returns>
    public static string InstanceToXml<T>(object instance)
    {
      return InstanceToXml<T>(typeof(XmlSerializer), instance, new UTF8Encoding());
    }


    /// <summary>
    /// Serializes an instance to xml using UTF8 encoding and the WCF DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="instance">The instance to be serialized.</param>
    /// <returns>Returns a xml string.</returns>
    public static string WcfInstanceToXml<T>(object instance)
    {
      return InstanceToXml<T>(typeof(DataContractSerializer), instance, new UTF8Encoding());
    }

    /// <summary>
    /// Serializes an instance to xml using UTF8 encoding and the WCF DataContractSerializer.
    /// </summary>
    /// <param name="T">The class type</typeparam>
    /// <param name="instance">The instance to be serialized.</param>
    /// <returns>Returns a xml string.</returns>
    public static string WcfInstanceToXml(Type type, object instance)
    {
      return InstanceToXml(typeof(DataContractSerializer), type, instance, new UTF8Encoding());
    }

    /// <summary>
    /// Deserializes  xml to an instance using the WCF DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="instance">The xml to be deserialized.</param>
    /// <returns>Returns an instance of the type.</returns>
    public static T WcfXmlToInstance<T>(string xml)
    {
      DataContractSerializer serializer = new DataContractSerializer(typeof(T));
      return (T)serializer.ReadObject(new XmlTextReader(new StringReader(xml)));
    }

    /// <summary>
    /// Deserializes  xml file to an instance using the WCF DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="filename">The full path and file name to be deserialized.</param>
    /// <returns>Returns an instance of the type.</returns>
    public static T WcfXmlFileToInstance<T>(string filename)
    {
      StreamReader reader = new StreamReader(filename, new UTF8Encoding());
      try
      {
        DataContractSerializer serializer = new DataContractSerializer(typeof(T));
        return (T)serializer.ReadObject(reader.BaseStream);
      }
      finally
      {
        reader.Close();
      }
    }

    /// <summary>
    /// Serializes an instance to an xml file using the WCF DataContractSerializer.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="filename">The full path and file name to be deserialized.</param>
    public static void WcfInstanceToXmlFile(object instance, string filename)
    {
      StreamWriter writer = new StreamWriter(filename, false, new UTF8Encoding());
      try
      {
        DataContractSerializer serializer = new DataContractSerializer(instance.GetType());
        serializer.WriteObject(writer.BaseStream, instance);
      }
      finally
      {
        writer.Close();
      }
    }
   

    /// <summary>
    /// Serializes an instance to xml.
    /// </summary>
    /// <param name="serializerType">The type of Serializer class to use.</param>
    /// 	<param name="type">The type of class.</param>
    /// <param name="instance">The instance to be serialized.</param>
    /// <param name="encoding">The character encoding  to be used.</param>
    /// <returns>Returns a xml string.</returns>
    public static string InstanceToXml(Type serializerType, Type type, object instance, Encoding encoding)
    {
      object serializer = DomainsManager.Instance.CreateInstance(serializerType, new object[] { type });
      MemoryStream stream = new MemoryStream();
      XmlTextWriter writer = new XmlTextWriter(stream, null);
      try
      {
        if (serializer is DataContractSerializer)
          ((DataContractSerializer)serializer).WriteObject(stream, instance);
        else
          if (serializer is XmlSerializer)
            ((XmlSerializer)serializer).Serialize(writer, instance);
          else
            throw new StatProBaseException("GeneralHelper: Unrecognized serializer specified to convert the instance to Xml format.", Severity.Abort);

        return encoding.GetString(stream.ToArray());
      }
      finally
      {
        if (stream != null)
          stream.Close();
        if (writer != null)
          writer.Close();
      }
    }

    /// <summary>
    /// Serializes an instance to xml.
    /// </summary>
    /// <typeparam name="T">The class type</typeparam>
    /// <param name="serializerType">The type of Serializer class to use.</param>
    /// <param name="instance">The instance to be serialized.</param>
    /// <param name="instance">The character encoding  to be used.</param>
    /// <returns>Returns a xml string.</returns>
    public static string InstanceToXml<T>(Type serializerType, object instance, Encoding encoding)
    {
      return InstanceToXml(serializerType, typeof(T), instance, encoding);
    }

    /// <summary>
    /// Get root and all inner exception messages
    /// </summary>
    /// <param name="exception">The root exception</param>
    /// <returns>Returns a concatenated string of all exception messages</returns>
    public static string GetExceptionMessages(Exception exception)
    {
      Exception anException = exception;
      StringBuilder stringBuilder = new StringBuilder();

      while (anException != null)
      {
        stringBuilder.AppendLine(anException.Message);
        anException = anException.InnerException;
      }

      return stringBuilder.ToString();
    }

    public static void ShowWaitCursor()
    {
      Cursor.Current = Cursors.WaitCursor;
    }

    public static void ShowDefaultCursor()
    {
      Cursor.Current = Cursors.Default;
    }
      */
		#endregion

		public static string ConstructFullyQualifiedPath(string relativePath)
		{
			return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath.Replace(".\\", string.Empty));
		}

		/// <summary>
		/// Finds a string within a string using a regular expression.
		/// </summary>
		/// <param name="pattern">The regular expression.</param>
		/// <param name="patternToken">The name of the token in the regular expression.</param>
		/// <param name="input">The input string that will be searched.</param>
		/// <returns>Returns the found string or empty if not found.</returns>
		public static string FindWithRegularExpression(string pattern, string patternToken, string input)
		{
			var result = string.Empty;
			var rg = new Regex(pattern, RegexOptions.IgnoreCase);
			var match = rg.Match(input);
			if (match != null && match.Groups[patternToken].Success && match.Groups.Count > 0)
				result = match.Groups[patternToken].Value;
			return result;
		}

		/// <summary>
		/// Retrieve list of Ip Addresses for hostname (DNS can be configured for multiple Ip addresses for a host)
		/// </summary>
		/// <param name="hostName">The host name</param>
		/// <returns>Returns a list of Ip addresses</returns>
		public static List<IPAddress> GetIpAddress(string hostName)
		{
			try
			{
				return new List<IPAddress>(Dns.GetHostAddresses(hostName));
			}
			catch
			{
				return new List<IPAddress>();
			}
		}

	}
}
