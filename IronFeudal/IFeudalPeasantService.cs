using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace IronFeudal
{
	[ServiceContract(SessionMode = SessionMode.Required)]
	public interface IFeudalPeasantService
	{
		[OperationContract]
		void Connect();

		[OperationContract(IsInitiating = false, IsTerminating = true)]
		void Disconnect();

		[OperationContract(IsInitiating = false)]
		FileMetaData ReadFileMetaData(string fileId);

		[OperationContract(IsInitiating = false)]
		string CreateFileStart(string fileId, FileMetaData metaData);

		[OperationContract(IsInitiating = false)]
		string ChangeFileStart(string fileId, FileMetaData metaData);

		[OperationContract(IsInitiating = false)]
		void SendFileData(string blockId, byte[] data);

		[OperationContract(IsInitiating = false)]
		void CreateFileEnd(string transferId);

		[OperationContract(IsInitiating = false)]
		void ChangeFileEnd(string transferId);

		[OperationContract(IsInitiating = false)]
		void DeleteFile(string fileId);
	}

	[DataContract]
	public class FileMetaData
	{
		[DataMember]
		public string FileId { get; set; }

		[DataMember]
		public Dictionary<string, string> FileHashList { get; set; }
	}
}
