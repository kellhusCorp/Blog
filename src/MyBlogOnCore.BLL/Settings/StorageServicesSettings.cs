namespace MyBlogOnCore.BLL.Settings;

public class StorageServicesSettings
{
    public string ImagesRootDirectory { get; set; } = null!;

    public string FilesRootDirectory { get; set; } = null!;

    public string InvariantImageRootDirectory => GetInvariantPath(ImagesRootDirectory);

    public string InvariantFilesRootDirectory => GetInvariantPath(FilesRootDirectory);

    private static string GetInvariantPath(string path)
    {
        if (Path.IsPathRooted(path))
        {
            return path
                .TrimStart(Path.DirectorySeparatorChar)
                .TrimStart(Path.AltDirectorySeparatorChar)
                .Replace('/', Path.DirectorySeparatorChar);
        }

        return path;
    }
}