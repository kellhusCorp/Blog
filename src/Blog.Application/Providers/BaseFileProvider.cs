namespace Blog.Application.Providers;

public abstract class BaseFileProvider : IFileProvider
{
    private readonly string _pathToContentRootDirectory;
    private readonly string _baseDirectory;

    protected BaseFileProvider(string pathToContentRootDirectory, string baseDirectory)
    {
        _pathToContentRootDirectory = pathToContentRootDirectory;
        _baseDirectory = baseDirectory;
    }

    public async Task<byte[]> GetFileAsync(string fileName)
    {
        fileName = GetFilePath(fileName);

        if (File.Exists(fileName))
        {
            return await File.ReadAllBytesAsync(fileName);
        }

        throw new FileNotFoundException();
    }

    public async Task AddFileAsync(string fileName, byte[] file)
    {
        var directoryPath = GetDirectory();

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        fileName = GetFilePath(fileName);

        await File.WriteAllBytesAsync(fileName, file);
    }

    public Task DeleteFileAsync(string fileName)
    {
        fileName = this.GetFilePath(fileName);

        if (File.Exists(fileName))
        {
            File.Delete(fileName);
        }

        return Task.CompletedTask;
    }

    private string GetDirectory()
    {
        return Path.Combine(
            _pathToContentRootDirectory,
            _baseDirectory);
    }

    private string GetFilePath(string fileName)
    {
        if (fileName.Contains("../")
            || fileName.Contains("..\\")
            || fileName.IndexOfAny(Path.GetInvalidPathChars()) > -1)
        {
            throw new ArgumentException("Filename contains invalid path characters.", nameof(fileName));
        }

        return Path.Combine(
            GetDirectory(),
            fileName);
    }
}