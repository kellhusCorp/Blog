using Blog.BLL.Providers;
using Microsoft.Extensions.Options;

namespace Blog.Application.Providers;

public class BlogFileProvider : BaseFileProvider, IBlogFileProvider
{
    public BlogFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantFilesRootDirectory)
    {
    }
}