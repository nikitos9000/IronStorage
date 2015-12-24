using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace IronPeasant.ServiceSystem
{
	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "PeasantData", Namespace = "http://schemas.datacontract.org/2004/07/IronLord")]
	public class PeasantData : object, IExtensibleDataObject
	{
		public ExtensionDataObject ExtensionData { get; set; }
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "FileData", Namespace = "http://schemas.datacontract.org/2004/07/IronLord")]
	public class FileData : object, IExtensibleDataObject
	{
		[DataMember]
		public string DirectoryName { get; set; }

		[DataMember]
		public string FileId { get; set; }

		[DataMember]
		public string FileName { get; set; }

		[DataMember]
		public long FileSize { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "TransferData", Namespace = "http://schemas.datacontract.org/2004/07/IronLord")]
	public class TransferData : object, IExtensibleDataObject
	{
		[DataMember]
		public string FileId { get; set; }

		[DataMember]
		public NodeData[] NodeList { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "NodeData", Namespace = "http://schemas.datacontract.org/2004/07/IronLord")]
	public class NodeData : object, IExtensibleDataObject
	{
		[DataMember]
		public string Authorization { get; set; }

		[DataMember]
		public string IpAddress { get; set; }

		[DataMember]
		public int Port { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "ReadData", Namespace = "http://schemas.datacontract.org/2004/07/IronLord")]
	public class ReadData : object, IExtensibleDataObject
	{
		public ExtensionDataObject ExtensionData { get; set; }
	}

	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[ServiceContract(ConfigurationName = "ILordPeasantService", SessionMode = SessionMode.Required)]
	public interface ILordPeasantService
	{
		[OperationContract(Action = "http://tempuri.org/ILordPeasantService/Login",
			ReplyAction = "http://tempuri.org/ILordPeasantService/LoginResponse")]
		PeasantData Login();

		[OperationContract(IsTerminating = true, IsInitiating = false,
			Action = "http://tempuri.org/ILordPeasantService/Logout",
			ReplyAction = "http://tempuri.org/ILordPeasantService/LogoutResponse")]
		void Logout();

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/StartFileTransferAdd",
			ReplyAction = "http://tempuri.org/ILordPeasantService/StartFileTransferAddResponse")]
		TransferData StartFileTransferAdd(FileData fileData);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/StartFileTransferChange",
			ReplyAction = "http://tempuri.org/ILordPeasantService/StartFileTransferChangeResponse")]
		TransferData StartFileTransferChange(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/StartFileTransferDelete",
			ReplyAction = "http://tempuri.org/ILordPeasantService/StartFileTransferDeleteResponse")]
		TransferData StartFileTransferDelete(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/StartFileTransferMove",
			ReplyAction = "http://tempuri.org/ILordPeasantService/StartFileTransferMoveResponse")]
		TransferData StartFileTransferMove(string fileId, string newFilePath);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/EndFileTransfer",
			ReplyAction = "http://tempuri.org/ILordPeasantService/EndFileTransferResponse")]
		void EndFileTransfer(TransferData transferData);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/StartFileRead",
			ReplyAction = "http://tempuri.org/ILordPeasantService/StartFileReadResponse")]
		ReadData StartFileRead(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/EndFileRead",
			ReplyAction = "http://tempuri.org/ILordPeasantService/EndFileReadResponse")]
		void EndFileRead(ReadData readData);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/GetFileInfos",
			ReplyAction = "http://tempuri.org/ILordPeasantService/GetFileInfosResponse")]
		FileData[] GetFileInfos();

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/ILordPeasantService/GetFileInfoById",
			ReplyAction = "http://tempuri.org/ILordPeasantService/GetFileInfoByIdResponse")]
		FileData GetFileInfoById(string fileId);
	}

	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public interface ILordPeasantServiceChannel : ILordPeasantService, IClientChannel
	{
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public class LordPeasantServiceClient : ClientBase<ILordPeasantService>, ILordPeasantService
	{
		public LordPeasantServiceClient()
		{
		}

		public LordPeasantServiceClient(string endpointConfigurationName) :
			base(endpointConfigurationName)
		{
		}

		public LordPeasantServiceClient(string endpointConfigurationName, string remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public LordPeasantServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public LordPeasantServiceClient(Binding binding, EndpointAddress remoteAddress) :
			base(binding, remoteAddress)
		{
		}

		public PeasantData Login()
		{
			return Channel.Login();
		}

		public void Logout()
		{
			Channel.Logout();
		}

		public TransferData StartFileTransferAdd(FileData fileData)
		{
			return Channel.StartFileTransferAdd(fileData);
		}

		public TransferData StartFileTransferChange(string fileId)
		{
			return Channel.StartFileTransferChange(fileId);
		}

		public TransferData StartFileTransferDelete(string fileId)
		{
			return Channel.StartFileTransferDelete(fileId);
		}

		public TransferData StartFileTransferMove(string fileId, string newFilePath)
		{
			return Channel.StartFileTransferMove(fileId, newFilePath);
		}

		public void EndFileTransfer(TransferData transferData)
		{
			Channel.EndFileTransfer(transferData);
		}

		public ReadData StartFileRead(string fileId)
		{
			return Channel.StartFileRead(fileId);
		}

		public void EndFileRead(ReadData readData)
		{
			Channel.EndFileRead(readData);
		}

		public FileData[] GetFileInfos()
		{
			return Channel.GetFileInfos();
		}

		public FileData GetFileInfoById(string fileId)
		{
			return Channel.GetFileInfoById(fileId);
		}
	}
}