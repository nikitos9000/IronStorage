using IronPeasant.NetworkSystem;

namespace IronPeasant.FileSystem
{
	public interface IFileNodeHandler
	{
		IFileHandler FileHandler { get; }

		void Transfer(INetworkNodeHandler networkNodeHandler);
	}
}