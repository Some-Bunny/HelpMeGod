using System;
using System.IO;

namespace SimpleFileBrowser
{
	
	public struct FileSystemEntry
	{
		

		public bool IsDirectory
		{
			get
			{
				return (this.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
			}
		}

		 
		public FileSystemEntry(string path, string name, bool isDirectory)
		{
			this.Path = path;
			this.Name = name;
			this.Extension = (isDirectory ? null : System.IO.Path.GetExtension(name));
			this.Attributes = (isDirectory ? FileAttributes.Directory : FileAttributes.Normal);
		}

		 
		public FileSystemEntry(FileSystemInfo fileInfo)
		{
			this.Path = fileInfo.FullName;
			this.Name = fileInfo.Name;
			this.Extension = fileInfo.Extension;
			this.Attributes = fileInfo.Attributes;
		}

		
		public readonly string Path;

		
		public readonly string Name;

		
		public readonly string Extension;

		
		public readonly FileAttributes Attributes;
	}
}
