using System;

namespace IronFeudal
{
	public class FeudalPeasantService : IFeudalPeasantService
	{
		public void Connect()
		{
			throw new NotImplementedException();
		}

		public void Disconnect()
		{
			throw new NotImplementedException();
		}

		public FileMetaData ReadFileMetaData(string fileId)
		{
			throw new NotImplementedException();
		}

		public string CreateFileStart(string fileId, FileMetaData metaData)
		{
			throw new NotImplementedException();
		}

		public string ChangeFileStart(string fileId, FileMetaData metaData)
		{
			throw new NotImplementedException();
		}

		public void SendFileData(string blockId, byte[] data)
		{
			throw new NotImplementedException();
		}

		public void CreateFileEnd(string transferId)
		{
			throw new NotImplementedException();
		}

		public void ChangeFileEnd(string transferId)
		{
			throw new NotImplementedException();
		}

		public void DeleteFile(string fileId)
		{
			throw new NotImplementedException();
		}
	}
}
