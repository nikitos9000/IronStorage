using System.IO;
using IronPeasant.FileSystem;
using IronPeasant.NetworkSystem;

namespace IronPeasant
{
	public class PublicDirectoryHandler : DirectoryHandler
	{
		public PublicDirectoryHandler(INetworkHandler networkHandler) : base(@"C:\Source", networkHandler)
		{
		}

		protected override void FileSystemFileCreated(string fileName)
		{
			INetworkHandler networkHandler = new NetworkHandler();
			FileInfo fileInfo = new FileInfo(DirectoryName + "/" + fileName);

			FileHandler fileHandler = new FileHandler();
			fileHandler.FileName = fileName;
			fileHandler.FileSize = fileInfo.Length;
			networkHandler.FileEvent(fileHandler);
		}

		protected override void FileSystemFileChanged(string fileName)
		{
		}

		protected override void FileSystemFileDeleted(string fileName)
		{
		}

		protected override void FileSystemFileRenamed(string oldFileName, string newFileName)
		{
		}
	}
}
