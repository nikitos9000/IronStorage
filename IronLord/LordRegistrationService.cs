using System;
using IronLord.Database;

namespace IronLord
{
	public class LordRegistrationService : ILordRegistrationService
	{
		private readonly IronDatabaseDataContext context;
		private readonly UserManager userManager;

		public LordRegistrationService()
		{
			context = new IronDatabaseDataContext();
			userManager = new UserManager(context);
		}

		public bool RegisterUser(string email, string password)
		{
			context.Log = Console.Out;
			Guid userId = userManager.CreateUser(email, password);
			return userId != Guid.Empty;
		}

		public bool RegisterUserConfirm(string code)
		{
			return true;
		}

		public bool UnregisterUser(string email, string password)
		{
			return userManager.DeleteUser(email, password);
		}
	}
}
