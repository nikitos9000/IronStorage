using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace IronLord
{
	[ServiceContract(SessionMode = SessionMode.NotAllowed)]
    public interface ILordFeudalService
    {
		[OperationContract]
    	Guid RegisterNode(string email, string password);

		[OperationContract]
    	FeudalInfo GetInfo(string email, string password, Guid id);
    }

	[DataContract]
	public class FeudalInfo
	{
		[DataMember]
		public Guid Id { get; set; }

		[DataMember]
		public Guid UserId { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int ProvidedQuota { get; set; }

		[DataMember]
		public int ProvidedQuotaUsed { get; set; }

		[DataMember]
		public byte[] ActionTokenKey { get; set; }

		[DataMember]
		public FeudalFileInfo[] Files { get; set; }
	}

	[DataContract]
	public class FeudalFileInfo
	{
		[DataMember]
		public Guid FileId { get; set; }

		[DataMember]
		public byte[] FileHash { get; set; }

		[DataMember]
		public bool Deleted { get; set; }
	}
}