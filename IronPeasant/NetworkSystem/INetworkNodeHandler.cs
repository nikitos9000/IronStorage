namespace IronPeasant.NetworkSystem
{
	public interface INetworkNodeHandler
	{
		void SendDataBlock(byte[] data);
	}
}