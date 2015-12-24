namespace IronPeasant.FileSystem
{
	public interface IFileHandler
	{
		string FileName { get; }
		long FileSize { get; }

		string Metadata { get; }

		IFileNodeHandler CreateNodeHandler();
	}
}