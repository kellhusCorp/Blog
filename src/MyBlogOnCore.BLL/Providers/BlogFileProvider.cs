using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MyBlogOnCore.BLL.Settings;

namespace MyBlogOnCore.BLL.Providers;

public class BlogFileProvider : BaseFileProvider, IBlogFileProvider
{
    public BlogFileProvider(
        IHostEnvironment hostEnvironment,
        IOptionsMonitor<StorageServicesSettings> servicesSettings)
        : base(hostEnvironment, servicesSettings.CurrentValue.InvariantFilesRootDirectory)
    {
    }
}