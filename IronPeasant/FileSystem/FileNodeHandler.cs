using IronPeasant.NetworkSystem;

namespace IronPeasant.FileSystem
{
	public class FileNodeHandler : IFileNodeHandler
	{
		public IFileHandler FileHandler { get; set; }

		public void Transfer(INetworkNodeHandler networkNodeHandler)
		{
//			networkNodeHandler.SendDataBlock();
		}
	}
}