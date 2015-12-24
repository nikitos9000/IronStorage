using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace IronPeasant.ServiceSystem
{
	[DebuggerStepThrough]
	[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
	[DataContract(Name = "FileMetaData", Namespace = "http://schemas.datacontract.org/2004/07/IronFeudal")]
	public class FileMetaData : object, IExtensibleDataObject
	{
		[DataMember]
		public string[] FileHashList { get; set; }

		[DataMember]
		public string FileId { get; set; }

		public ExtensionDataObject ExtensionData { get; set; }
	}

	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	[ServiceContract(ConfigurationName = "IFeudalPeasantService", SessionMode = SessionMode.Required)]
	public interface IFeudalPeasantService
	{
		[OperationContract(Action = "http://tempuri.org/IFeudalPeasantService/Connect",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/ConnectResponse")]
		void Connect();

		[OperationContract(IsTerminating = true, IsInitiating = false,
			Action = "http://tempuri.org/IFeudalPeasantService/Disconnect",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/DisconnectResponse")]
		void Disconnect();

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/SendByteBlock",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/SendByteBlockResponse")]
		void SendByteBlock(byte[] data);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/CreateFileStart",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/CreateFileStartResponse")]
		string CreateFileStart(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/ChangeFileStart",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/ChangeFileStartAddResponse")]
		string ChangeFileStart(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/CreateFileEnd",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/CreateFileEndResponse")]
		void CreateFileEnd(string transferId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/ChangeFileEnd",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/ChangeFileEndResponse")]
		void ChangeFileEnd(string transferId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/DeleteFile",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/DeleteFileResponse")]
		void DeleteFile(string fileId);

		[OperationContract(IsInitiating = false, Action = "http://tempuri.org/IFeudalPeasantService/ReadFileMetaData",
			ReplyAction = "http://tempuri.org/IFeudalPeasantService/ReadFileMetaDataResponse")]
		FileMetaData ReadFileMetaData(string fileId);
	}

	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public interface IFeudalPeasantServiceChannel : IFeudalPeasantService, IClientChannel
	{
	}

	[DebuggerStepThrough]
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public class FeudalPeasantServiceClient : ClientBase<IFeudalPeasantService>, IFeudalPeasantService
	{
		public FeudalPeasantServiceClient()
		{
		}

		public FeudalPeasantServiceClient(string endpointConfigurationName) :
			base(endpointConfigurationName)
		{
		}

		public FeudalPeasantServiceClient(string endpointConfigurationName, string remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public FeudalPeasantServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress) :
			base(endpointConfigurationName, remoteAddress)
		{
		}

		public FeudalPeasantServiceClient(Binding binding, EndpointAddress remoteAddress) :
			base(binding, remoteAddress)
		{
		}

		public void Connect()
		{
			Channel.Connect();
		}

		public void Disconnect()
		{
			Channel.Disconnect();
		}

		public void SendByteBlock(byte[] data)
		{
			Channel.SendByteBlock(data);
		}

		public string CreateFileStart(string fileId)
		{
			return Channel.CreateFileStart(fileId);
		}

		public string ChangeFileStart(string fileId)
		{
			return Channel.ChangeFileStart(fileId);
		}

		public void CreateFileEnd(string transferId)
		{
			Channel.CreateFileEnd(transferId);
		}

		public void ChangeFileEnd(string transferId)
		{
			Channel.ChangeFileEnd(transferId);
		}

		public void DeleteFile(string fileId)
		{
			Channel.DeleteFile(fileId);
		}

		public FileMetaData ReadFileMetaData(string fileId)
		{
			return Channel.ReadFileMetaData(fileId);
		}
	}
}
