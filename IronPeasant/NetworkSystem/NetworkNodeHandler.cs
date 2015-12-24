using IronPeasant.ServiceSystem;

namespace IronPeasant.NetworkSystem
{
	public class NetworkNodeHandler : INetworkNodeHandler
	{
		private readonly IFeudalPeasantService feudalPeasantService;

		public NetworkNodeHandler(IFeudalPeasantService feudalPeasantService)
		{
			this.feudalPeasantService = feudalPeasantService;
		}

		public void SendDataBlock(byte[] data)
		{
			feudalPeasantService.SendByteBlock(data);
		}
	}
}