using Blog.BLL.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Blog.BLL.Providers;

public class BlogFileProvider : BaseFileProvider, IBlogFileProvider
{
    public BlogFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantFilesRootDirectory)
    {
    }
}