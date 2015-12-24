using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using IronLord.Database;

namespace IronLord
{
	internal class UserManager
	{
		private readonly IronDatabaseDataContext context;
		private readonly MD5 hasher = MD5.Create();

		public UserManager(IronDatabaseDataContext context)
		{
			this.context = context;
		}

		public Guid CreateUser(string email, string password)
		{
			if (HasUser(email)) return Guid.Empty;

			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

			User user = new User();
			user.EMailAddress = email;
			user.PasswordHash = hasher.ComputeHash(passwordBytes);
			user.Quota = 0;
			user.QuotaUsed = 0;

			context.Users.InsertOnSubmit(user);
			context.SubmitChanges();

			return user.Id;
		}

		public bool DeleteUser(string email, string password)
		{
			Guid? userId = VerifyUser(email, password);
			if (userId == Guid.Empty) return false;

			context.Users.DeleteOnSubmit(context.Users.Single(u => u.Id == userId));
			context.SubmitChanges();

			return true;
		}

		public Guid VerifyUser(string email, string password)
		{
			User user = context.Users.SingleOrDefault(u => u.EMailAddress == email);
			if (user == null) return Guid.Empty;

			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			byte[] passwordHash = hasher.ComputeHash(passwordBytes);

			if (!user.PasswordHash.Equals(passwordHash)) return Guid.Empty;

			return user.Id;
		}

		public bool HasUser(string email)
		{
			return context.Users.Any(u => u.EMailAddress == email);
		}
	}
}
