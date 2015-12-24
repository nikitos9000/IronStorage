using System;
using System.Net.Security;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace IronLord
{
	[ServiceContract(SessionMode = SessionMode.Required, ProtectionLevel = ProtectionLevel.Sign, CallbackContract = typeof (ILordPeasantCallback))]
	public interface ILordPeasantService
	{
		[OperationContract]
		Guid Login(string email, string password);

		[OperationContract(IsInitiating = false, IsTerminating = true)]
		void Logout();

		[OperationContract(IsInitiating = false)]
		PeasantFileInfo StartFileCreate(string fileName, Guid directoryId, long fileSize, byte[] fileHash);

		[OperationContract(IsInitiating = false)]
		void EndFileCreate(PeasantFileResult result);

		[OperationContract(IsInitiating = false)]
		PeasantFileInfo StartFileChange(Guid fileId, string fileName, Guid directoryId, long fileSize, byte[] fileHash);

		[OperationContract(IsInitiating = false)]
		void EndFileChange(PeasantFileResult result);

		[OperationContract(IsInitiating = false)]
		PeasantFileInfo StartFileDelete(Guid fileId);

		[OperationContract(IsInitiating = false)]
		void EndFileDelete(PeasantFileResult result);

		[OperationContract(IsInitiating = false)]
		PeasantDirectoryInfo StartDirectoryCreate(string name, Guid parentId, bool isPublic, bool isVersioned);

		[OperationContract(IsInitiating = false)]
		void EndDirectoryCreate(PeasantDirectoryResult result);

		[OperationContract(IsInitiating = false)]
		PeasantDirectoryInfo StartDirectoryChange(Guid id, bool isPublic, bool isVersioned);

		[OperationContract(IsInitiating = false)]
		void EndDirectoryChange(PeasantDirectoryResult result);

		[OperationContract(IsInitiating = false)]
		PeasantDirectoryInfo StartDirectoryDelete(Guid id);

		[OperationContract(IsInitiating = false)]
		void EndDirectoryDelete(PeasantDirectoryResult result);
	}

	public interface ILordPeasantCallback
	{
		[OperationContract(IsOneWay = true)]
		void OnFileChanged();

		[OperationContract(IsOneWay = true)]
		void OnDirectoryChanged();

		[OperationContract(IsOneWay = true)]
		void OnActionBegin();

		[OperationContract(IsOneWay = true)]
		void OnActionEnd();
	}

	[DataContract]
	public class PeasantNodeInfo
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public byte[] IpAddress { get; set; }

		[DataMember]
		public int Port { get; set; }

		[DataMember]
		public byte[] ActionToken { get; set; }
	}

	[DataContract]
	public class PeasantNodeResult
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public bool Success { get; set; }
	}

	[DataContract]
	public class PeasantFileInfo
	{
		[DataMember]
		public Guid ActionId { get; set; }

		[DataMember]
		public Guid FileId { get; set; }

		[DataMember]
		public PeasantNodeInfo[] Nodes { get; set; }
	}

	[DataContract]
	public class PeasantFileResult
	{
		[DataMember]
		public Guid ActionId { get; set; }

		[DataMember]
		public Guid FileId { get; set; }

		[DataMember]
		public PeasantNodeResult[] Nodes { get; set; }
	}

	[DataContract]
	public class PeasantDirectoryInfo
	{
		[DataMember]
		public Guid ActionId { get; set; }

		[DataMember]
		public Guid DirectoryId { get; set; }

		[DataMember]
		public PeasantNodeInfo[] Nodes { get; set; }
	}

	[DataContract]
	public class PeasantDirectoryResult
	{
		[DataMember]
		public Guid ActionId { get; set; }

		[DataMember]
		public Guid DirectoryId { get; set; }

		[DataMember]
		public PeasantNodeResult[] Nodes { get; set; }
	}
}
