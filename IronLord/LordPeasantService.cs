using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using IronLord.Database;

namespace IronLord
{
	public class LordPeasantService : ILordPeasantService
	{
		private static readonly IDictionary<Guid, List<ILordPeasantCallback>> callbacks = new Dictionary<Guid, List<ILordPeasantCallback>>();

		private readonly IronDatabaseDataContext context;
		private readonly UserManager userManager;
		private ILordPeasantCallback callback;

		private List<ILordPeasantCallback> userCallbacks;
		private Guid userId;

		public LordPeasantService()
		{
			context = new IronDatabaseDataContext();
			userManager = new UserManager(context);
			context.Log = Console.Out; //
		}

		public Guid Login(string email, string password)
		{
			userId = userManager.VerifyUser(email, password);
			if (userId == Guid.Empty) return Guid.Empty;

			callback = OperationContext.Current.GetCallbackChannel<ILordPeasantCallback>();

			lock (callbacks)
				userCallbacks = callbacks[userId] ?? (callbacks[userId] = new List<ILordPeasantCallback>());

			lock (userCallbacks)
				userCallbacks.Add(callback);

			return userId;
		}

		public void Logout()
		{
			lock (userCallbacks)
				userCallbacks.Remove(callback);
		}

		public PeasantFileInfo StartFileCreate(string fileName, Guid directoryId, long fileSize, byte[] fileHash)
		{
			PeasantFileInfo fileInfo = new PeasantFileInfo();

			using (var transaction = context.Connection.BeginTransaction(IsolationLevel.Serializable))
			{
				context.Transaction = transaction;

				bool fileExists = context.Files.Any(f => f.FileName == fileName && f.DirectoryId == directoryId);
				if (fileExists) return null;

				File file = new File();
				file.FileName = fileName;
				file.DirectoryId = directoryId;
				file.IsLocked = true;

				context.Files.InsertOnSubmit(file);
				context.SubmitChanges();

				FileVersion fileVersion = new FileVersion();
				fileVersion.FileId = file.Id;
				fileVersion.DeltaSize = fileSize;
				fileVersion.Fileize = fileSize;
				fileVersion.FileHash = fileHash;

				context.FileVersions.InsertOnSubmit(fileVersion);
				context.SubmitChanges();

				fileInfo.ActionId = FileActionBegin(file.Id);
				fileInfo.FileId = file.Id;

				fileInfo.Nodes = new PeasantNodeInfo[0]; //Fill nodes

				transaction.Commit();
			}

			Notify(x => x.OnActionBegin());

			return fileInfo;
		}

		public void EndFileCreate(PeasantFileResult result)
		{
			EndFileUpdate(result);
		}

		public PeasantFileInfo StartFileChange(Guid fileId, string fileName, Guid directoryId, long fileSize, byte[] fileHash)
		{
			if (fileId == Guid.Empty) throw new ArgumentNullException("fileId");
			if (fileName == null) throw new ArgumentNullException("fileName");
			if (fileSize < 0) throw new ArgumentNullException("fileSize");
			if (fileHash == null) throw new ArgumentNullException("fileHash");

			PeasantFileInfo fileInfo = new PeasantFileInfo();

			using (var transaction = context.Connection.BeginTransaction(IsolationLevel.Serializable))
			{
				context.Transaction = transaction;

				if (FileActionCheck(fileId)) return null;

				File file = context.Files.Single(f => f.Id == fileId);
				file.IsLocked = true;
				file.FileName = fileName;
				file.DirectoryId = directoryId;

				FileVersion currentFileVersion = file.FileVersions.Last();

				if (fileSize != currentFileVersion.Fileize || !fileHash.SequenceEqual(currentFileVersion.FileHash.ToArray()))
				{
					Directory directory = file.Directory; //Вот тут аккуратнее с этим directoryId
					if (directory.IsVersioned)
					{
						FileVersion fileVersion = new FileVersion();
						fileVersion.FileId = fileId;
						fileVersion.Fileize = fileSize;
						fileVersion.DeltaSize = Math.Abs(fileSize - currentFileVersion.Fileize);
						fileVersion.FileHash = fileHash;

						context.FileVersions.InsertOnSubmit(fileVersion);
					}
					else
					{
						currentFileVersion.Fileize = fileSize;
						currentFileVersion.DeltaSize = fileSize;
						currentFileVersion.FileHash = fileHash;
					}
				}

				context.SubmitChanges();

				fileInfo.ActionId = FileActionBegin(fileId);
				fileInfo.FileId = fileId;
				fileInfo.Nodes = CreateNodeInfos(GetFileNodes(fileId)).ToArray();

				transaction.Commit();
			}

			Notify(x => x.OnActionBegin());

			return fileInfo;
		}

		public void EndFileChange(PeasantFileResult result)
		{
			EndFileUpdate(result);
		}

		public PeasantFileInfo StartFileDelete(Guid fileId)
		{
			PeasantFileInfo fileInfo = new PeasantFileInfo();

			using (var transaction = context.Connection.BeginTransaction(IsolationLevel.Serializable))
			{
				context.Transaction = transaction;

				if (FileActionCheck(fileId)) return null;

				File file = context.Files.Single(f => f.Id == fileId);
				file.IsLocked = true;

				var fileNodes = context.FileNodes.Where(x => x.FileId == fileId);
				foreach(var fileNode in fileNodes)
					fileNode.IsDeleted = true;

				context.SubmitChanges();

				fileInfo.ActionId = FileActionBegin(fileId);
				fileInfo.FileId = fileId;
				fileInfo.Nodes = CreateNodeInfos(GetFileNodes(fileId)).ToArray();

				transaction.Commit();
			}

			Notify(x => x.OnActionBegin());

			return fileInfo;
		}

		public void EndFileDelete(PeasantFileResult result)
		{
			EndFileUpdate(result);
		}

		public PeasantDirectoryInfo StartDirectoryCreate(string name, Guid parentId, bool isPublic, bool isVersioned)
		{
			throw new NotImplementedException();
		}

		public void EndDirectoryCreate(PeasantDirectoryResult result)
		{
			throw new NotImplementedException();
		}

		public PeasantDirectoryInfo StartDirectoryChange(Guid id, bool isPublic, bool isVersioned)
		{
			throw new NotImplementedException();
		}

		public void EndDirectoryChange(PeasantDirectoryResult result)
		{
			throw new NotImplementedException();
		}

		public PeasantDirectoryInfo StartDirectoryDelete(Guid id)
		{
			throw new NotImplementedException();
		}

		public void EndDirectoryDelete(PeasantDirectoryResult result)
		{
			throw new NotImplementedException();
		}

		private void EndFileUpdate(PeasantFileResult result)
		{
			using (var transaction = context.Connection.BeginTransaction(IsolationLevel.Serializable))
			{
				context.Transaction = transaction;

				File file = context.Files.Single(f => f.Id == result.FileId);
				file.IsLocked = false;

				context.SubmitChanges();

				FileActionEnd(result.ActionId);

				transaction.Commit();
			}

			Notify(x => x.OnActionEnd());
		}

		private IEnumerable<Node> GetNewFileNodes(long fileSize, Guid? fileId)
		{
			return context.Nodes.Where(x => x.ProvidedQuota - x.ProvidedQuotaUsed > fileSize).Take(123);
		}

		private IEnumerable<Node> GetFileNodes(Guid fileId)
		{
			return context.Nodes.Join(context.FileNodes.Where(fn => fn.FileId == fileId), n => n.Id, fn => fn.NodeId, (n, fn) => n);
		}

		private static IEnumerable<PeasantNodeInfo> CreateNodeInfos(IEnumerable<Node> nodes)
		{
			return nodes.Select(x => new PeasantNodeInfo { Id = x.Id, IpAddress = x.IpAddress.ToArray(), Port = 123, ActionToken = CreateActionToken(x, null, null, null)});
		}

		private static byte[] CreateActionToken(Node node, File file, FileVersion fileVersion, FileAction fileAction)
		{
			node.Id.ToByteArray();
			file.Id.ToByteArray();
			fileVersion.Id.ToByteArray();
			BitConverter.GetBytes(fileVersion.Fileize);
			fileVersion.FileHash.ToArray();
			fileAction.Id.ToByteArray();
			BitConverter.GetBytes(fileAction.ActionType);

			return new byte[0];
		}

		private Guid FileActionBegin(Guid fileId)
		{
			if (FileActionCheck(fileId)) return Guid.Empty;

			FileAction fileAction = new FileAction();
			fileAction.FileId = fileId;
			fileAction.ActionType = 123;

			context.FileActions.InsertOnSubmit(fileAction);
			context.SubmitChanges();

			return fileAction.Id;
		}

		private bool FileActionEnd(Guid actionId)
		{
			FileAction fileAction = context.FileActions.SingleOrDefault(a => a.Id == actionId);
			if (fileAction == null) return false;

			context.FileActions.DeleteOnSubmit(fileAction);
			context.SubmitChanges();
			return true;
		}

		private bool FileActionCheck(Guid fileId)
		{
			return context.FileActions.Any(a => a.FileId == fileId);
		}

		private void Notify(Action<ILordPeasantCallback> action)
		{
			lock (userCallbacks)
			{
				userCallbacks.RemoveAll(x => ((ICommunicationObject) x).State != CommunicationState.Opened);
				userCallbacks.ForEach(action);
			}
		}
	}
}
