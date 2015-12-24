using System;
using System.IO;
using IronPeasant.NetworkSystem;

namespace IronPeasant
{
	public abstract class DirectoryHandler
	{
		private readonly string directoryName;
		private readonly FileSystemWatcher fileSystemWatcher;
		private readonly INetworkHandler networkHandler;

		protected DirectoryHandler(string directoryName, INetworkHandler networkHandler)
		{
			this.directoryName = directoryName;
			this.networkHandler = networkHandler;

			fileSystemWatcher = new FileSystemWatcher(directoryName);
			fileSystemWatcher.Created += (sender, eventArgs) => FileSystemFileCreated(eventArgs.Name);
			fileSystemWatcher.Changed += (sender, eventArgs) => FileSystemFileChanged(eventArgs.Name);
			fileSystemWatcher.Deleted += (sender, eventArgs) => FileSystemFileDeleted(eventArgs.Name);
			fileSystemWatcher.Renamed += (sender, eventArgs) => FileSystemFileRenamed(eventArgs.OldName, eventArgs.Name);
			fileSystemWatcher.IncludeSubdirectories = true;
			fileSystemWatcher.EnableRaisingEvents = true;
		}

		public string DirectoryName
		{
			get { return directoryName; }
		}

		public INetworkHandler NetworkHandler
		{
			get { return networkHandler; }
		}

		protected abstract void FileSystemFileCreated(String fileName);

		protected abstract void FileSystemFileChanged(String fileName);

		protected abstract void FileSystemFileDeleted(String fileName);

		protected abstract void FileSystemFileRenamed(String oldFileName, String newFileName);
	}
}
