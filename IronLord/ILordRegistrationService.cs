using System.ServiceModel;

namespace IronLord
{
	[ServiceContract(SessionMode = SessionMode.NotAllowed)]
	public interface ILordRegistrationService
	{
		[OperationContract]
		bool RegisterUser(string email, string password);

		[OperationContract]
		bool RegisterUserConfirm(string code);
	}
}
