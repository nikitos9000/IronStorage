namespace IronPeasant.FileSystem
{
	public class FileHandler : IFileHandler
	{
		public string FileName { get; set; }

		public long FileSize { get; set; }

		public string Metadata { get; set; }

		public IFileNodeHandler CreateNodeHandler()
		{
			return null;
		}
	}
}