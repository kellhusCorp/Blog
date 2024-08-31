using Blog.Application.Settings;
using Microsoft.Extensions.Options;

namespace Blog.Application.Providers;

public class BlogFileProvider : BaseFileProvider, IBlogFileProvider
{
    public BlogFileProvider(
        string pathToContentRootDirectory,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(pathToContentRootDirectory, servicesSettings.CurrentValue.InvariantFilesRootDirectory)
    {
    }
}