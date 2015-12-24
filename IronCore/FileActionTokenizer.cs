using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IronCore
{
	public class FileActionToken
	{
		public Guid NodeId { get; set; }
		public Guid FileId { get; set; }
		public Guid FileVersionId { get; set; }
		public Guid FileActionId { get; set; }
		public long FileSize { get; set; }
		public byte[] FileHash { get; set; }
		public int ActionType { get; set; }
	}

	public class FileActionTokenizer
	{
		private const int RSA_KEY_SIZE = 512;
		private readonly RSACryptoServiceProvider crypto = new RSACryptoServiceProvider(RSA_KEY_SIZE);
		private readonly BinaryFormatter formatter = new BinaryFormatter();

		byte[] PackFileActionToken(FileActionToken token, byte[] key)
		{
//			RSAParameters pa = new RSAParameters();
			
			//			crypto.ImportParameters();

//			IEnumerable<byte> result = token.NodeId.ToByteArray()
//				.Concat(token.FileId.ToByteArray())
//				.Concat(token.FileVersionId.ToByteArray())
//				.Concat(BitConverter.GetBytes(token.FileSize))
//				.Concat(BitConverter.GetBytes(token.FileHash.Length))
//				.Concat(token.FileHash)
//				.Concat(token.FileActionId.ToByteArray())
//				.Concat(BitConverter.GetBytes(token.ActionType)););

			using (MemoryStream stream = new MemoryStream())
			{
				formatter.Serialize(stream, token);
				return stream.ToArray();
			}
		}

		FileActionToken UnpackFileActionToken(byte[] token, byte[] key)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				stream.Write(token, 0, token.Length);
				stream.Position = 0;
				return (FileActionToken) formatter.Deserialize(stream);
			}
		}
	}
}
