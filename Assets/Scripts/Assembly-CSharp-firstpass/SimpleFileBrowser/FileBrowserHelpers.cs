using System;
using System.IO;
using UnityEngine;

namespace SimpleFileBrowser
{
	
	public static class FileBrowserHelpers
	{
		 
		public static bool FileExists(string path)
		{
			return File.Exists(path);
		}

		 
		public static bool DirectoryExists(string path)
		{
			return Directory.Exists(path);
		}

		 
		public static bool IsDirectory(string path)
		{
			bool flag = Directory.Exists(path);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = File.Exists(path);
				if (flag2)
				{
					result = false;
				}
				else
				{
					string extension = Path.GetExtension(path);
					result = (extension == null || extension.Length <= 1);
				}
			}
			return result;
		}

		 
		public static FileSystemEntry[] GetEntriesInDirectory(string path)
		{
			FileSystemEntry[] result2;
			try
			{
				FileSystemInfo[] items = new DirectoryInfo(path).GetFileSystemInfos();
				FileSystemEntry[] result = new FileSystemEntry[items.Length];
				for (int i = 0; i < items.Length; i++)
				{
					result[i] = new FileSystemEntry(items[i]);
				}
				result2 = result;
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				result2 = null;
			}
			return result2;
		}

		 
		public static string CreateFileInDirectory(string directoryPath, string filename)
		{
			string path = Path.Combine(directoryPath, filename);
			using (File.Create(path))
			{
			}
			return path;
		}

		 
		public static string CreateFolderInDirectory(string directoryPath, string folderName)
		{
			string path = Path.Combine(directoryPath, folderName);
			Directory.CreateDirectory(path);
			return path;
		}

		 
		public static void WriteBytesToFile(string targetPath, byte[] bytes)
		{
			File.WriteAllBytes(targetPath, bytes);
		}

		 
		public static void WriteTextToFile(string targetPath, string text)
		{
			File.WriteAllText(targetPath, text);
		}

		 
		public static void WriteCopyToFile(string targetPath, string sourceFile)
		{
			File.Copy(sourceFile, targetPath, true);
		}

		 
		public static void AppendBytesToFile(string targetPath, byte[] bytes)
		{
			using (FileStream stream = new FileStream(targetPath, FileMode.Append, FileAccess.Write))
			{
				stream.Write(bytes, 0, bytes.Length);
			}
		}

		 
		public static void AppendTextToFile(string targetPath, string text)
		{
			File.AppendAllText(targetPath, text);
		}

		 
		public static void AppendCopyToFile(string targetPath, string sourceFile)
		{
			using (Stream input = File.OpenRead(sourceFile))
			{
				using (Stream output = new FileStream(targetPath, FileMode.Append, FileAccess.Write))
				{
					byte[] buffer = new byte[4096];
					int bytesRead;
					while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
					{
						output.Write(buffer, 0, bytesRead);
					}
				}
			}
		}

		 
		public static byte[] ReadBytesFromFile(string sourcePath)
		{
			return File.ReadAllBytes(sourcePath);
		}

		 
		public static string ReadTextFromFile(string sourcePath)
		{
			return File.ReadAllText(sourcePath);
		}

		 
		public static void ReadCopyFromFile(string sourcePath, string destinationFile)
		{
			File.Copy(sourcePath, destinationFile, true);
		}

		 
		public static string RenameFile(string path, string newName)
		{
			string newPath = Path.Combine(Path.GetDirectoryName(path), newName);
			File.Move(path, newPath);
			return newPath;
		}

		
		public static string RenameDirectory(string path, string newName)
		{
			string newPath = Path.Combine(new DirectoryInfo(path).Parent.FullName, newName);
			Directory.Move(path, newPath);
			return newPath;
		}

		
		public static void DeleteFile(string path)
		{
			File.Delete(path);
		}

		
		public static void DeleteDirectory(string path)
		{
			Directory.Delete(path, true);
		}

		
		public static string GetFilename(string path)
		{
			return Path.GetFileName(path);
		}

		
		public static long GetFilesize(string path)
		{
			return new FileInfo(path).Length;
		}

		
		public static DateTime GetLastModifiedDate(string path)
		{
			return new FileInfo(path).LastWriteTime;
		}
	}
}
