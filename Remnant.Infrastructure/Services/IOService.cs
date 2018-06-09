using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Remnant.Core.Services
{

	/// <summary>
	/// The Helper assists with IO and file routines
	/// </summary>
	/// <remarks>
	/// Author: Neill Verreynne
	/// Date: Mid 2009
	/// </remarks>
	public static class IOService
	{

		/// <summary>
		/// Copies all files and subfolders
		/// </summary>
		/// <param name="source">Source path</param>
		/// <param name="target">Target path</param>
		/// <param name="overwrite">Overwite if files exist in target (default=false)</param>
		/// <returns></returns>
		public static int CopyFolder(DirectoryInfo source, DirectoryInfo target, bool overwrite = false)
		{
			var count = 0;

			foreach (var dir in source.GetDirectories())
				count = count + CopyFolder(dir, target.CreateSubdirectory(dir.Name), overwrite);

			foreach (var file in source.GetFiles())
			{
				file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
				count++;
			}

			return count;
		}

		/// <summary>
		/// Deletes a directory recursively if exists
		/// </summary>
		/// <param name="directoryPath">The path of the directory</param>
		public static void DeleteDirectory(string directoryPath)
		{
			if (Directory.Exists(directoryPath))
				Directory.Delete(directoryPath, true);
		}

		/// <summary>
		/// Remove directory/file readon only attribute 
		/// </summary>
		/// <param name="dirInfo">The directory</param>
		/// <param name="recursively">Specify if subdirectories msut be changed as well recursively</param>
		public static void ClearReadOnlyAttribute(DirectoryInfo dirInfo, bool recursively = true)
		{
			if (dirInfo != null)
			{
				dirInfo.Attributes &= ~FileAttributes.ReadOnly;

				foreach (var fileInfo in dirInfo.GetFiles())
					fileInfo.Attributes &= ~FileAttributes.ReadOnly;
				
				if (recursively)
				{
					foreach (var subDirInfo in dirInfo.GetDirectories())
						ClearReadOnlyAttribute(subDirInfo);
				}
			}
		}

		/// <summary>
		/// Validates and gets the version (major/minor/build) of an assembly's file
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns>Returns the full version</returns>
		public static string GetFileVersion(Assembly assembly)
		{
			const string versionExpression = @"(?<Major>[0-9]+)[.](?<Minor>[0-9]+)[.](?<Build>[0-9]+)";
			var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			var versionMatch = Regex.Match(versionInfo.FileVersion, versionExpression);
			if (versionMatch.Success)
				return versionMatch.Groups["Major"].Value + "." + versionMatch.Groups["Minor"].Value + "." + versionMatch.Groups["Build"].Value;
			return string.Empty;
		}

		public static void SaveFile(string fullPathName, byte[] bytes)
		{
			File.WriteAllBytes(fullPathName, bytes);
		}

		public static void SaveFile(string fullPathName, string content)
		{
			File.WriteAllText(fullPathName, content);
		}

		public static void SaveFile(string path, string fileName, string content)
		{
			File.WriteAllText(Path.Combine(path, fileName), content);
		}


		public static void SaveFileToTempFolder(string path, byte[] bytes)
		{
			SaveFile(Path.GetTempPath() + path, bytes);
		}

		public static void LaunchFileFromTempFolder(string path)
		{
			Process.Start(Path.GetTempPath() + path);
		}

		public static Process LaunchFile(string fullPathFileName)
		{
			return Process.Start(fullPathFileName);
		}

		public static Process LaunchFile(ProcessStartInfo startInfo)
		{
			return Process.Start(startInfo);
		}

		public static void DeleteFile(string fullPathFileName)
		{
			File.Delete(fullPathFileName);
		}


		public static string ExtractDirectory(string fullPathFileName)
		{
			return Path.GetDirectoryName(fullPathFileName);
		}


		/// <summary>
		/// Get the filename without extension from the full path
		/// </summary>
		/// <param name="fullPathFileName"></param>
		/// <returns></returns>
		public static string ExtractFileName(string fullPathFileName)
		{
			return Path.GetFileNameWithoutExtension(fullPathFileName);
		}

		/// <summary>
		/// Get the filename with extension from the full path
		/// </summary>
		/// <param name="fullPathFileName"></param>
		/// <returns></returns>
		public static string ExtractFileNameWithExtension(string fullPathFileName)
		{
			return Path.GetFileName(fullPathFileName);
		}

		public static string ExtractFileExtension(string fullPathFileName)
		{
			return Path.GetExtension(fullPathFileName);
		}

		public static bool FileExists(string file)
		{
			return File.Exists(file);
		}
		/// <summary>
		/// Creates the directory if it doesnt exist
		/// </summary>
		/// <param name="directory">The directory path</param>
		public static void EnforceDirectory(string directory)
		{
			if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
				Directory.CreateDirectory(directory);
		}

		public static string GetDirectory(string fullPathName)
		{
			return Path.GetDirectoryName(fullPathName);
		}

		public static void MoveFile(string file, string toDirectory, bool enforceDirectory = true)
		{
			Shield.AgainstNullOrEmpty(file).Raise();
			Shield.AgainstNullOrEmpty(toDirectory).Raise();

			if (File.Exists(file))
			{
				if (enforceDirectory)
					EnforceDirectory(toDirectory);
				File.Move(file, toDirectory + "/" + Path.GetFileName(file));
			}
			else
				throw new FileNotFoundException(
					string.Format("The file '{0}' that you requst for moving to directory {1} cannot be found.", file, toDirectory));
		}

		public static string ReadTextFile(string file)
		{
			Shield.AgainstNullOrEmpty(file).Raise();
			if (File.Exists(file))
				return File.ReadAllText(file);

			throw new FileNotFoundException(string.Format("The file '{0}' that you requst for reading cannot be found.", file));
		}

		/// <summary>
		/// Creates a relative path from one file or folder to another.
		/// </summary>
		/// <param name="fromPath">Contains the directory that defines the start of the relative path.</param>
		/// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
		/// <returns>The relative path from the start directory to the end path.</returns>
		/// <exception cref="ArgumentNullException"></exception>
		public static String GetRelativePath(string fromPath, string toPath)
		{
			Shield.AgainstNullOrEmpty(fromPath).Raise();
			Shield.AgainstNullOrEmpty(toPath).Raise();

			var fromUri = new Uri(fromPath);
			var toUri = new Uri(toPath);
			var relativeUri = fromUri.MakeRelativeUri(toUri);
			var relativePath = Uri.UnescapeDataString(relativeUri.ToString());
			return relativePath.Replace('/', Path.DirectorySeparatorChar);
		}
	}
}
