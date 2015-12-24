using IronPeasant.FileSystem;

namespace IronPeasant.NetworkSystem
{
	public interface INetworkHandler
	{
		void FileEvent(IFileHandler fileHandler);
	}
}