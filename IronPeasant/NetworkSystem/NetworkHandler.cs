using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using IronPeasant.FileSystem;
using IronPeasant.ServiceSystem;

namespace IronPeasant.NetworkSystem
{
	public class NetworkHandler : INetworkHandler
	{
		private readonly ILordPeasantService lordPeasantService = new LordPeasantServiceClient();

		public void FileEvent(IFileHandler fileHandler)
		{
			FileData fileData = new FileData();
			fileData.FileName = fileHandler.FileName;
			fileData.FileSize = fileHandler.FileSize;

			lordPeasantService.Login();
			TransferData transferData = lordPeasantService.StartFileTransferAdd(fileData);

			ProcessTask(transferData.NodeList, nodeData => FileCreateTask(fileHandler, transferData, nodeData));

			lordPeasantService.EndFileTransfer(transferData);
			lordPeasantService.Logout();
		}

		public void FileCreateEvent(IFileHandler fileHandler)
		{
			lordPeasantService.Login();
			
			lordPeasantService.Logout();
		}

		public void FileChangeEvent(IFileHandler fileHandler)
		{
			lordPeasantService.Login();

			lordPeasantService.Logout();
		}

		public void FileRenameEvent(string oldFileName, string newFileName)
		{
			lordPeasantService.Login();

			lordPeasantService.Logout();
		}

		public void FileDeleteEvent(string fileName)
		{
			lordPeasantService.Login();
			TransferData transferData = lordPeasantService.StartFileTransferDelete(fileName);

			ProcessTask(transferData.NodeList, nodeData => FileDeleteTask(transferData, nodeData));

			lordPeasantService.Logout();
		}

		private static void ProcessTask(ICollection<NodeData> nodeList, Action<NodeData> action)
		{
			TaskFactory taskFactory = Task.Factory;
			Task[] tasks = new Task[nodeList.Count];

			int taskIndex = 0;
			foreach (var nodeData in nodeList)
			{
				NodeData localNodeData = nodeData;

				Task task = taskFactory.StartNew(() => action(localNodeData), TaskCreationOptions.LongRunning);
				tasks[taskIndex++] = task;
			}
			Task.WaitAll(tasks.ToArray());
		}

		private static void FileCreateTask(IFileHandler fileHandler, TransferData transferData, NodeData nodeData)
		{
			IFeudalPeasantService feudalPeasantService = GetNodeService(nodeData);
			feudalPeasantService.Connect();
			string transferId = feudalPeasantService.CreateFileStart(transferData.FileId);

			IFileNodeHandler fileNodeHandler = fileHandler.CreateNodeHandler();
			fileNodeHandler.Transfer(new NetworkNodeHandler(feudalPeasantService));

			feudalPeasantService.CreateFileEnd(transferId);
			feudalPeasantService.Disconnect();
		}

		private static void FileChangeTask(IFileHandler fileHandler, TransferData transferData, NodeData nodeData)
		{
			IFeudalPeasantService feudalPeasantService = GetNodeService(nodeData);
			feudalPeasantService.Connect();
			string transferId = feudalPeasantService.ChangeFileStart(transferData.FileId);

			IFileNodeHandler fileNodeHandler = fileHandler.CreateNodeHandler();
			fileNodeHandler.Transfer(new NetworkNodeHandler(feudalPeasantService));

			feudalPeasantService.ChangeFileEnd(transferId);
			feudalPeasantService.Disconnect();
		}

		private static void FileDeleteTask(TransferData transferData, NodeData nodeData)
		{
			IFeudalPeasantService feudalPeasantService = GetNodeService(nodeData);
			feudalPeasantService.Connect();
			feudalPeasantService.DeleteFile(transferData.FileId);
			feudalPeasantService.Disconnect();
		}

		private static IFeudalPeasantService GetNodeService(NodeData nodeData)
		{
			EndpointAddress endpoint =
				new EndpointAddress(String.Format("http://{0}:{1}/Design_Time_Addresses/IronFeudal/FeudalPeasantService/", nodeData.IpAddress, nodeData.Port));
			return new FeudalPeasantServiceClient(new WSHttpBinding("WSHttpBinding_IFeudalPeasantService"), endpoint);
		}
	}
}
